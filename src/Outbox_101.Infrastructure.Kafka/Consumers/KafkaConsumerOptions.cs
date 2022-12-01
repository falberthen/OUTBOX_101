namespace Outbox_101.Infrastructure.Kafka.Consumers;

public class KafkaConsumerOptions
{
    public string[]? Topics { get; set; }
    public string ConnectionString { get; set; }
    public string Group { get; set; }
}