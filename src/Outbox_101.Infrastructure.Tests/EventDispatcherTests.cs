namespace Outbox_101.Infrastructure.Tests;

public class EventDispatcherTests
{
    private sealed record FakeEvent(Guid TicketId) : EventBase;

    private sealed class FakeHandler : IEventHandler<FakeEvent>
    {
        public FakeEvent? Received { get; private set; }

        public Task HandleAsync(FakeEvent @event, CancellationToken cancellationToken)
        {
            Received = @event;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task DispatchAsync_ResolvesHandlerByConcreteType_AndInvokesIt()
    {
        var handler = new FakeHandler();
        var services = new ServiceCollection()
            .AddScoped<IEventHandler<FakeEvent>>(_ => handler)
            .BuildServiceProvider();

        var dispatcher = new EventDispatcher(
            services.GetRequiredService<IServiceScopeFactory>(),
            Substitute.For<ILogger<EventDispatcher>>());

        var @event = new FakeEvent(Guid.NewGuid());
        await dispatcher.DispatchAsync(@event, CancellationToken.None);

        handler.Received.Should().Be(@event);
    }
}
