namespace Outbox_101.EventConsumer;

internal class TicketOpenHandler : IEventHandler<TicketOpen>
{
    private readonly ITicketUnitOfWork _unitOfWork;

    public TicketOpenHandler(ITicketUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(TicketOpen created, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetByIdAsync(created.Ticket.Id);

        // Setting ticket as InProgress
        ticket.SetInProgress();

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.SaveChangesAsync();
    }
}
