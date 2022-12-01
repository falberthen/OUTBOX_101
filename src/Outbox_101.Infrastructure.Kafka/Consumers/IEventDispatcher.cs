using Outbox_101.Domain.Tickets.Events.Base;

namespace Outbox_101.Infrastructure.Kafka.Consumers;

public interface IEventDispatcher
{
    Task DispatchAsync(EventBase @event, CancellationToken cancellationToken);
}
