using Outbox_101.Domain.Tickets.Events.Base;

namespace Outbox_101.Domain.Base;

public interface IAggregateRoot
{
    Guid Id { get; }
    void AppendEvent(EventBase @event);
    IEnumerable<EventBase> GetUncommittedEvents();
    void ClearUncommittedEvents();
}
