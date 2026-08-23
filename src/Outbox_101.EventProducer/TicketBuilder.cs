namespace Outbox_101.EventProducer;

public static class TicketBuilder
{
    public static async Task Build(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            // Generating a ticket
            var unitOfWork = scope.ServiceProvider.GetRequiredService<ITicketUnitOfWork>();
            var ticket = Ticket.OpenNew("A new ticket", "fix the damn bug!", TicketPriority.HIGH);
            await unitOfWork.Tickets.AddAsync(ticket);
            await unitOfWork.SaveChangesAsync();
        }
    }
}