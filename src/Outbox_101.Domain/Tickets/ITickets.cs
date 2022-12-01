namespace Outbox_101.Domain.Tickets;

public interface ITickets
{
    Task AddAsync(Ticket ticket);
    Task<Ticket> GetByIdAsync(Guid ticketId);
    void Update(Ticket ticket);
}