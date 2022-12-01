using Newtonsoft.Json;
using Outbox_101.Domain.Base;

namespace Outbox_101.Infrastructure.Outbox;

public static class OutboxMessageExtension
{
    public static IEnumerable<OutboxMessage> OutboxUncommitedEvents(this IAggregateRoot aggregateRoot)
    {
        var events = aggregateRoot!.GetUncommittedEvents()
            .Select(e => e)
            .ToList();

        foreach (var @event in events)
        {
            var payload = JsonConvert.SerializeObject(@event);
            var outboxMessage = new OutboxMessage(payload, @event.GetType().Name);
            yield return outboxMessage;
        }
    }
}
