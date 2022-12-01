using Microsoft.EntityFrameworkCore;
using Outbox_101.Domain.Tickets;
using Outbox_101.Infrastructure.Outbox;

namespace Outbox_101.Infrastructure.Persistence;

public class TicketsDbContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public TicketsDbContext(DbContextOptions<TicketsDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
