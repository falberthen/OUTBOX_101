using Polly;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Outbox_101.Infrastructure.Kafka.Consumers.Serialization;
using Outbox_101.Domain.Tickets.Events.Base;

namespace Outbox_101.Infrastructure.Kafka.Consumers;

public class KafkaConsumer : IKafkaConsumer
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IConsumer<string, EventBase> _consumer;
    private readonly ILogger<KafkaConsumer> _logger;

    public KafkaConsumer(
        IEventDispatcher eventDispatcher,
        JsonEventSerializer<EventBase> serializer,
        IOptions<KafkaConsumerOptions> kafkaConsumerConfig,
        ILogger<KafkaConsumer> logger)
    {
        _logger = logger;

        if (kafkaConsumerConfig is null)
            throw new ArgumentNullException(nameof(kafkaConsumerConfig));

        _eventDispatcher = eventDispatcher;

        var config = kafkaConsumerConfig.Value;
        var consumerConfig = new ConsumerConfig
        {
            GroupId = config.Group,
            BootstrapServers = config.ConnectionString,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
        
        _consumer = new ConsumerBuilder<string, EventBase>(consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(serializer!)
            .Build();

        _consumer.Subscribe(config.Topics);
    }

    public async Task StartConsumeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Kafka consumer started.");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
                await ConsumeNextMessage(_consumer, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred when consuming a Kafka message: {Message} {StackTrace}", e.Message, e.StackTrace);
            _consumer.Close();
        }
    }

    private async Task ConsumeNextMessage(IConsumer<string, EventBase> consumer, CancellationToken cancellationToken)
    {
        var policy = Policy
           .Handle<Exception>()
           .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        await policy.ExecuteAsync(
           async () =>
           {
               await Task.Yield();
               var result = _consumer.Consume(cancellationToken);
               var @event = result.Message.Value;

               if (@event is null)
               {
                   _logger.LogError("Unable to deserialize integration event.", consumer);
                   await Task.CompletedTask;
               }

               _logger.LogInformation("Dispatching event: {event}", @event);
               await _eventDispatcher.DispatchAsync(@event!, cancellationToken);
               consumer.Commit();
           });
    }
}
