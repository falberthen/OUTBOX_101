using Microsoft.EntityFrameworkCore;
using Outbox_101.Domain.Tickets;

namespace Outbox_101.Infrastructure.Persistence.Repositories;

public class Tickets : ITickets
{
    private readonly TicketsDbContext _dbContext;

    public Tickets(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Ticket ticket)
    {
        await _dbContext.AddAsync(ticket);
    }

    public async Task<Ticket> GetByIdAsync(Guid ticketId)
    {
        return await _dbContext.Tickets
            .FirstAsync(t => t.Id == ticketId);
    }

    public void Update(Ticket ticket)
    {
        _dbContext.Update(ticket);
    }
}
