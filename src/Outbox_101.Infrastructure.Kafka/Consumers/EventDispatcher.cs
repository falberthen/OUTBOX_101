using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Outbox_101.Domain.Tickets.Events.Base;

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
            var scope = _serviceScopeFactory.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var mediator = scopedServices.GetRequiredService<IMediator>();

            await mediator.Publish(@event, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred when publishing event: {Message} {StackTrace}", e.Message, e.StackTrace);
        }
    }
}