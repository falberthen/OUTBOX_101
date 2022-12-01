using Outbox_101.Domain.Tickets.Events.Base;

namespace Outbox_101.Infrastructure.Kafka.Producers;

public interface IKafkaProducer
{
    Task PublishAsync(EventBase @event, CancellationToken cancellationToken = default);
}