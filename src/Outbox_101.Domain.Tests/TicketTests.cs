namespace Outbox_101.Domain.Tests;

public class TicketTests
{
    [Fact]
    public void OpenNewTicket_ShouldSetOpenStatus()
    {
        // Arrange
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;

        // Act
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // Assert
        Assert.NotNull(ticket);
        ticket.Id.Should().NotBe(Guid.Empty);
        ticket.Title.Should().Be(title);
        ticket.Description.Should().Be(description);
        ticket.Priority.Should().Be(ticketPriority);
        ticket.Status.Should().Be(TicketStatus.OPEN);
    }

    [Fact]
    public void SetTicketInProgress_ShouldHaveInProgressStatus()
    {
        // Arrange
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // Act
        ticket.SetInProgress();

        // Assert
        Assert.NotNull(ticket);
        ticket.Status.Should().Be(TicketStatus.IN_PROGRESS);
    }

    [Fact]
    public void SetTicketClosed_ShouldHaveClosedStatus()
    {
        // Arrange
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;
        var ticket = Ticket.OpenNew(title, description, ticketPriority);
        ticket.SetInProgress();

        // Act
        ticket.Close();

        // Assert
        Assert.NotNull(ticket);
        ticket.Status.Should().Be(TicketStatus.CLOSED);
    }
}