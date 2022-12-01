using Confluent.Kafka;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Outbox_101.Domain.Tickets.Events.Base;
using System.Text;

namespace Outbox_101.Infrastructure.Kafka.Producers;

public class KafkaProducer : IKafkaProducer
{
    private readonly ILogger<KafkaProducer> _logger;
    private readonly KafkaProducerOptions _kafkaConfig;
    private IProducer<string, string> _producer;

    public KafkaProducer(IOptions<KafkaProducerOptions> producerConfigOptions, ILogger<KafkaProducer> logger)
    {
        _logger = logger;

        if (producerConfigOptions is null)
            throw new ArgumentNullException(nameof(producerConfigOptions));

        _kafkaConfig = producerConfigOptions.Value;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaConfig.ConnectionString,
        };

        var producerBuilder = new ProducerBuilder<string, string>(consumerConfig);
        _producer = producerBuilder.Build();
    }

    public async Task PublishAsync(EventBase @event, CancellationToken cancellationToken = default)
    {
        try
        {
            var eventTypeName = @event.GetType().Name;
            var headers = new Headers()
            {
                new Header("id", Encoding.ASCII.GetBytes(Guid.NewGuid().ToString())),
                new Header("eventType", Encoding.ASCII.GetBytes(eventTypeName))
            };
            
            var message = new Message<string, string>
            {
                Key = eventTypeName,
                Value = JsonConvert.SerializeObject(@event),
                Headers = headers
            };

            _logger.LogInformation("Publishing message {message} to topic {Topic}...", message, _kafkaConfig.Topic);
            await _producer.ProduceAsync(_kafkaConfig.Topic, message, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to produce Kafka message: {Message} {StackTrace}", e.Message, e.StackTrace);
            throw;
        }
    }
}
