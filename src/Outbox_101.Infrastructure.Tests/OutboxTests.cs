using Outbox_101.Domain.Tickets;
using Outbox_101.Infrastructure.Outbox;

namespace Outbox_101.Infrastructure.Tests;

public class OutboxTests
{
    [Fact]
    public void OutboxUncommitedEvents_ShouldReturnOutboxMessages_ForAllUncommitedEvents()
    {
        // Given
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // When
        var outboxMessages = ticket.OutboxUncommitedEvents();

        // Then
        Assert.NotNull(outboxMessages);
        ticket.GetUncommittedEvents().Count().Should().Be(outboxMessages.Count());        
    }
}