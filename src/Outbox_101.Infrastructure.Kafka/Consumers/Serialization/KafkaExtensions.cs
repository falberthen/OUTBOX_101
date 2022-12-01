using Confluent.Kafka;
using Newtonsoft.Json;
using Outbox_101.Domain.Tickets.Events.Base;

namespace Outbox_101.Infrastructure.Kafka.Consumers.Serialization;

public static class KafkaExtensions
{
    public static EventBase? ToEvent(this ConsumeResult<string, string> message)
    {
        if (message is null)
            return null;

        var eventType = TypeGetter.GetTypeFromCurrentDomainAssembly(message.Message.Key);

        if (eventType == null)
            return null;

        // deserialize event
        return JsonConvert.DeserializeObject(message.Message.Value, eventType) as EventBase;
    }
}   
