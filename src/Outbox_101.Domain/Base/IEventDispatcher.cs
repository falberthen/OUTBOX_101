namespace Outbox_101.Domain.Base;

public interface IEventDispatcher
{
    Task DispatchAsync(EventBase @event, CancellationToken cancellationToken);
}
