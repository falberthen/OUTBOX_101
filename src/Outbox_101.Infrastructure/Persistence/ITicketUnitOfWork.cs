using Outbox_101.Domain.Tickets;
using Outbox_101.Infrastructure.Outbox;

namespace Outbox_101.Infrastructure.Persistence;

public interface ITicketUnitOfWork
{
    ITickets Tickets { get; }
    IOutboxMessages OutboxMessages { get; }
    Task SaveChangesAsync();
}
