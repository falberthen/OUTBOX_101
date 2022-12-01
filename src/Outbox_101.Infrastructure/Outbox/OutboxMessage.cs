namespace Outbox_101.Infrastructure.Outbox;

public record OutboxMessage
{
    public Guid Id { get; }
    public string Payload { get; private set; }
    public string Type { get; private set; }
    public DateTime OcurredAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    private OutboxMessage() { }

    public OutboxMessage(string payload, string aggregateType)
    {
        Id = Guid.NewGuid();
        Payload = payload;
        Type = aggregateType;
        OcurredAt = DateTime.UtcNow;
    }

    public void SetAsProcessed()
    {
        ProcessedAt= DateTime.UtcNow;
    }
}
