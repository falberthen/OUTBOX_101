using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Outbox_101.Domain.Tickets.Events.Base;
using Outbox_101.Infrastructure.Persistence;
using Outbox_101.Infrastructure.Kafka.Producers;
using Outbox_101.Infrastructure.Kafka.Consumers.Serialization;
using Outbox_101.Infrastructure.Kafka.Producers.Serialization;

namespace Outbox_101.Infrastructure.Outbox.Polling;

public class OutboxMessageProcessor : IOutboxMessageProcessor
{
    private readonly ITicketUnitOfWork _unitOfWork;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly OutboxMessageProcessorOptions _processorOptions;
    private readonly ILogger<OutboxMessageProcessor> _logger;

    public OutboxMessageProcessor(
        ITicketUnitOfWork unitOfWork,
        IKafkaProducer kafkaProducer,
        IOptions<OutboxMessageProcessorOptions> options,
        ILogger<OutboxMessageProcessor> logger)
    {
        _unitOfWork = unitOfWork;
        _kafkaProducer = kafkaProducer;
        _processorOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ProcessMessagesAsync(CancellationToken cancellationToken)
    {
        var messages = await _unitOfWork.OutboxMessages
            .FetchUnprocessedAsync(_processorOptions.BatchSize, cancellationToken);

        var settings = new JsonSerializerSettings()
        {
            ContractResolver = new PrivateResolver(),
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        };

        foreach (var message in messages)
        {
            try
            {
                var eventType = TypeGetter.GetTypeFromCurrentDomainAssembly(message.Type);
                var @event = JsonConvert
                     .DeserializeObject(message.Payload, eventType, settings) as EventBase;

                // publishing to kafka
                await _kafkaProducer.PublishAsync(@event!, cancellationToken);
                // setting message as processed
                message.SetAsProcessed();

                _unitOfWork.OutboxMessages.Update(message);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                _logger.LogInformation("An error ocurred while processing message {message}", message);
                throw;
            }
        }
    }
}
