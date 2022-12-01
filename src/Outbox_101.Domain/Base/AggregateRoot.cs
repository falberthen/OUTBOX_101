using Outbox_101.Domain.Tickets.Events.Base;

namespace Outbox_101.Domain.Base;

public record AggregateRoot : IAggregateRoot
{
    public Guid Id { get; set; } = default!;
    public IEnumerable<EventBase> GetUncommittedEvents() => _uncommittedEvents;
    private readonly Queue<EventBase> _uncommittedEvents = new();

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    public void AppendEvent(EventBase @event) => _uncommittedEvents.Enqueue(@event);
}
