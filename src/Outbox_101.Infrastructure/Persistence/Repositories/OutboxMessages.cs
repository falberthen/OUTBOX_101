using Microsoft.EntityFrameworkCore;
using Outbox_101.Infrastructure.Outbox;

namespace Outbox_101.Infrastructure.Persistence.Repositories;

public class OutboxMessages : IOutboxMessages
{
    private readonly TicketsDbContext _dbContext;

    public OutboxMessages(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddRangeAsync(IEnumerable<OutboxMessage> outboxMessages)
    {
        await _dbContext.AddRangeAsync(outboxMessages);
    }

    public async Task<IReadOnlyList<OutboxMessage>> FetchUnprocessedAsync(int batchSize, CancellationToken cancellationToken)
    {
        var outboxMessages = await _dbContext.OutboxMessages
            .Where(m => null == m.ProcessedAt)
            .OrderBy(m => m.OcurredAt)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        return outboxMessages;
    }

    public void Update(OutboxMessage outboxMessage)
    {
        _dbContext.Update(outboxMessage);
    }
}