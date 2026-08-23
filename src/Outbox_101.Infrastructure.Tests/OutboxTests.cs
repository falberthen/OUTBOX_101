namespace Outbox_101.Infrastructure.Tests;

public class OutboxTests
{
    [Fact]
    public void OutboxUncommitedEvents_ShouldReturnOutboxMessages_ForAllUncommitedEvents()
    {
        // Arrange
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // Act
        var outboxMessages = ticket.OutboxUncommitedEvents();

        // Assert
        Assert.NotNull(outboxMessages);
        ticket.GetUncommittedEvents().Count().Should().Be(outboxMessages.Count());        
    }
}