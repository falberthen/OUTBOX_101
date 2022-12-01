namespace Outbox_101.Infrastructure.Workers.Outbox.Debezium;

public interface IDebeziumConnectorSetup
{
    Task StartConfiguringAsync(CancellationToken cancellationToken = default);
}
