namespace Outbox_101.Infrastructure.Outbox;

public interface IOutboxMessages
{
    Task AddRangeAsync(IEnumerable<OutboxMessage> outboxMessage);
    void Update(OutboxMessage outboxMessage);
    Task<IReadOnlyList<OutboxMessage>> FetchUnprocessedAsync(int batchSize, CancellationToken cancellationToken);
}
