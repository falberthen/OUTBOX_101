namespace Outbox_101.Domain.Base;

public interface IEventHandler
{
    Task HandleAsync(IDomainEvent @event, CancellationToken cancellationToken);
}

public interface IEventHandler<TEvent> : IEventHandler where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);

    Task IEventHandler.HandleAsync(IDomainEvent @event, CancellationToken cancellationToken)
        => HandleAsync((TEvent)@event, cancellationToken);
}
