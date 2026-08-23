namespace Outbox_101.Domain.Tickets.Events;

public record TicketInProgress(Ticket Ticket) : EventBase;