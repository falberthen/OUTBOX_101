namespace Outbox_101.Infrastructure.Kafka.Consumers;

public interface IKafkaConsumer
{
    Task StartConsumeAsync(CancellationToken cancellationToken = default);
}
