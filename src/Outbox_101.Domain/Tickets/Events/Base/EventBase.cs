using MediatR;

namespace Outbox_101.Domain.Tickets.Events.Base;

public abstract record EventBase : INotification
{
    public Guid Id { get; set; }
    public DateTime OccurredAt { get; set; }

    protected EventBase()
    {
        Id = Guid.NewGuid();
        OccurredAt = DateTime.UtcNow;
    }
}
