namespace Outbox_101.Infrastructure.Kafka.Producers;

public class KafkaProducerOptions
{
    public string Topic { get; set; }
    public string ConnectionString { get; set; }
}