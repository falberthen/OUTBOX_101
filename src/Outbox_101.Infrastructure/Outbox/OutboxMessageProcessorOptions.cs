namespace Outbox_101.Infrastructure.Outbox;

public class OutboxMessageProcessorOptions
{
    public TimeSpan Interval { get; set; }
    public int BatchSize { get; set; }
}