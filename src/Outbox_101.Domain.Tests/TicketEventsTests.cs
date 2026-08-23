namespace Outbox_101.Domain.Tests;

public class TicketEventsTests
{
    [Fact]
    public void OpenNewTicket_ShouldCauseTicketOpenEvent()
    {
        // Arrange
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;

        // Act
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // Assert
        Assert.NotNull(ticket);
        var @event = ticket.GetUncommittedEvents().LastOrDefault() as TicketOpen;
        Assert.NotNull(@event);
        @event.Should().BeOfType<TicketOpen>();
    }

    [Fact]
    public void SetTicketInProgress_ShouldCauseTicketInProgressEvent()
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
        var @event = ticket.GetUncommittedEvents().LastOrDefault() as TicketInProgress;
        Assert.NotNull(@event);
        @event.Should().BeOfType<TicketInProgress>();
    }

    [Fact]
    public void CloseTicket_ShouldCauseTicketClosedEvent()
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
        var @event = ticket.GetUncommittedEvents().LastOrDefault() as TicketClosed;
        Assert.NotNull(@event);
        @event.Should().BeOfType<TicketClosed>();
    }

    [Fact]
    public void RedeliveredTicketOpen_ShouldNotCauseSecondTicketInProgressEvent()
    {
        // Arrange
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;
        var ticket = Ticket.OpenNew(title, description, ticketPriority);
        ticket.SetInProgress();

        // Act
        ticket.SetInProgress();

        // Assert
        ticket.Status.Should().Be(TicketStatus.IN_PROGRESS);
        ticket.GetUncommittedEvents().OfType<TicketInProgress>().Should().HaveCount(1);
    }

    [Fact]
    public void InvalidTicketData_ShouldThrowArgumentNullException()
    {
        // Arrange
        string title = string.Empty;
        string description = string.Empty;
        
        // Act
        Func<Ticket> action = () =>
            Ticket.OpenNew(title, description, TicketPriority.LOW);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}