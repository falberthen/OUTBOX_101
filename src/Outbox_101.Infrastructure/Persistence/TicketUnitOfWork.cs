using Outbox_101.Domain.Base;
using Outbox_101.Domain.Tickets;
using Outbox_101.Infrastructure.Outbox;

namespace Outbox_101.Infrastructure.Persistence;

public class TicketUnitOfWork : ITicketUnitOfWork
{
    public ITickets Tickets { get; }
    public IOutboxMessages OutboxMessages { get; }
    private readonly TicketsDbContext _dbContext;

    public TicketUnitOfWork(
        TicketsDbContext dbContext,
        ITickets tickets,
        IOutboxMessages outboxMessages)
    {
        _dbContext = dbContext;
        Tickets = tickets ?? throw new ArgumentNullException(nameof(tickets));
        OutboxMessages = outboxMessages ?? throw new ArgumentNullException(nameof(outboxMessages));
    }

    public async Task SaveChangesAsync()
    {
        // getting tracked entities
        var aggregates = _dbContext.ChangeTracker.Entries()
            .Where(e => e.Entity is IAggregateRoot c && c.GetUncommittedEvents().Any())
            .Select(e => e.Entity as IAggregateRoot)
            .ToArray();

        foreach (var aggregate in aggregates)
        {
            var outboxMessages = aggregate!.OutboxUncommitedEvents()
                .ToArray();

            aggregate!.ClearUncommittedEvents();

            if (outboxMessages.Any())
                await OutboxMessages.AddRangeAsync(outboxMessages);
        }

        // commiting transaction
        await _dbContext.SaveChangesAsync();
    }
}
