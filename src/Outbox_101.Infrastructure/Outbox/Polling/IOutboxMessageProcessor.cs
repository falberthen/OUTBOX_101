namespace Outbox_101.Infrastructure.Outbox.Polling;

public interface IOutboxMessageProcessor
{
    Task ProcessMessagesAsync(CancellationToken cancellationToken);
}