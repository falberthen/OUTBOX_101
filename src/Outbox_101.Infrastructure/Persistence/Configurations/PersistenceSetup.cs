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