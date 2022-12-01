using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Outbox_101.Infrastructure.Persistence.Repositories;
using Outbox_101.Infrastructure.Outbox;
using Outbox_101.Domain.Tickets;

namespace Outbox_101.Infrastructure.Persistence.Configurations;

public static class PersistenceSetup
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddDbContext<TicketsDbContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            )
            .AddScoped<ITicketUnitOfWork, TicketUnitOfWork>()
            .AddScoped<ITickets, Tickets>()
            .AddScoped<IOutboxMessages, OutboxMessages>();
    }
}