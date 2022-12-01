namespace Outbox_101.Infrastructure.Workers.Outbox.Debezium;

public record DebeziumSettings
{
    public string ConnectorUrl { get; set; }
}