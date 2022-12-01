using Outbox_101.Domain.Tickets.Events.Base;

namespace Outbox_101.Domain.Tickets.Events;

public record TicketOpen(Ticket Ticket) : EventBase;