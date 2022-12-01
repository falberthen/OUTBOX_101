using Outbox_101.Domain.Base;
using Outbox_101.Domain.Tickets.Events;

namespace Outbox_101.Domain.Tickets;

public record Ticket : AggregateRoot
{
    public static Ticket OpenNew(string title, string description, TicketPriority priority)
    {
        if (string.IsNullOrEmpty(title))
            throw new ArgumentNullException(nameof(title));
        if (string.IsNullOrEmpty(description))
            throw new ArgumentNullException(nameof(description));

        return new Ticket(title, description, priority);
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public TicketStatus Status { get; private set; }
    public TicketPriority Priority { get; private set; }

    private Ticket(string title, string description, TicketPriority priority) 
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Priority = priority;
        Status = TicketStatus.OPEN;
        var @event = new TicketOpen(this);
        AppendEvent(@event);
    }

    public void SetInProgress()
    {
        Status = TicketStatus.IN_PROGRESS;
        var @event = new TicketInProgress(this);
        AppendEvent(@event);
    }

    public void Close()
    {
        Status = TicketStatus.CLOSED;
        var @event = new TicketClosed(this);
        AppendEvent(@event);
    }
    
    private Ticket() { }
}