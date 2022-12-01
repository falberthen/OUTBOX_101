using MediatR;
using Outbox_101.Domain.Tickets.Events;
using Outbox_101.Infrastructure.Persistence;

namespace Outbox_101.EventConsumer;

internal class TicketHandler :
    INotificationHandler<TicketOpen>,
    INotificationHandler<TicketInProgress>
{
    private readonly ITicketUnitOfWork _unitOfWork;

    public TicketHandler(
        ITicketUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TicketOpen created, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetByIdAsync(created.Ticket.Id);
        
        // Setting ticket as InProgress
        ticket.SetInProgress();

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task Handle(TicketInProgress inProgress, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets
            .GetByIdAsync(inProgress.Ticket.Id);

        // Setting ticket as Closed
        ticket.Close();

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.SaveChangesAsync();
    }
}
