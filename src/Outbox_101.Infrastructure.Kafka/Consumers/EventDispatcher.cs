namespace Outbox_101.Infrastructure.Kafka.Consumers;

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EventDispatcher> _logger;

    public EventDispatcher(IServiceScopeFactory serviceScopeFactory, ILogger<EventDispatcher> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task DispatchAsync(EventBase @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing event {@event}", @event);

        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());

            if (scope.ServiceProvider.GetService(handlerType) is not IEventHandler handler)
                throw new InvalidOperationException($"Handler for event {@event.GetType().Name} not registered.");

            await handler.HandleAsync(@event, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred when publishing event: {Message} {StackTrace}", e.Message, e.StackTrace);
        }
    }
}