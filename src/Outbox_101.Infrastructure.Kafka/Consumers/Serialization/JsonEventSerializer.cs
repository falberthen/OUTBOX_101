using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Outbox_101.Infrastructure.Kafka.Consumers.Serialization;

public class JsonEventSerializer<T> : IDeserializer<T?> 
    where T : class
{
    public T? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
            return null;

        var eventType = GetEventType(context);
        if (eventType == null)
            return null;

        var message = Encoding.UTF8.GetString(data);
        var @event = JsonConvert.DeserializeObject(message, eventType) as T;
        return @event;
    }

    public Type? GetEventType(SerializationContext context)
    {
        var eventTypeName = Encoding.UTF8.GetString(context.Headers.GetLastBytes("eventType"));
        return TypeGetter.GetTypeFromCurrentDomainAssembly(eventTypeName);
    }
}