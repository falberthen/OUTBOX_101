namespace Outbox_101.Infrastructure.Kafka.Producers;

public interface IKafkaProducer
{
    Task PublishAsync(EventBase @event, CancellationToken cancellationToken = default);
}