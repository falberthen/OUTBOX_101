namespace Outbox_101.EventConsumer;

internal class TicketInProgressHandler : IEventHandler<TicketInProgress>
{
    private readonly ITicketUnitOfWork _unitOfWork;

    public TicketInProgressHandler(ITicketUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(TicketInProgress inProgress, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetByIdAsync(inProgress.Ticket.Id);

        // Setting ticket as Closed
        ticket.Close();

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.SaveChangesAsync();
    }
}
