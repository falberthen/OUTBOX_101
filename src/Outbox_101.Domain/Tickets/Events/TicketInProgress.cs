using Outbox_101.Domain.Tickets.Events.Base;

namespace Outbox_101.Domain.Tickets.Events;

public record TicketInProgress(Ticket Ticket) : EventBase;