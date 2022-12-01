namespace Outbox_101.Infrastructure.Workers.Outbox.Polling;

public interface IOutboxMessageProcessingWorker
{
    Task StartProcessingAsync(CancellationToken cancellationToken = default);
}