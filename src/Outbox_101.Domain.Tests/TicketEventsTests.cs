using FluentAssertions;
using Outbox_101.Domain.Tickets;
using Outbox_101.Domain.Tickets.Events;

namespace Outbox_101.Domain.Tests;

public class TicketEventsTests
{
    [Fact]
    public void OpenNewTicket_ShouldCauseTicketOpenEvent()
    {
        // Given
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;

        // When
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // Then
        Assert.NotNull(ticket);
        var @event = ticket.GetUncommittedEvents().LastOrDefault() as TicketOpen;
        Assert.NotNull(@event);
        @event.Should().BeOfType<TicketOpen>();
    }

    [Fact]
    public void SetTicketInProgress_ShouldCauseTicketInProgressEvent()
    {
        // Given
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // When
        ticket.SetInProgress();

        // Then
        Assert.NotNull(ticket);
        var @event = ticket.GetUncommittedEvents().LastOrDefault() as TicketInProgress;
        Assert.NotNull(@event);
        @event.Should().BeOfType<TicketInProgress>();
    }

    [Fact]
    public void CloseTicket_ShouldCauseTicketClosedEvent()
    {
        // Given
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // When
        ticket.Close();

        // Then
        Assert.NotNull(ticket);
        var @event = ticket.GetUncommittedEvents().LastOrDefault() as TicketClosed;
        Assert.NotNull(@event);
        @event.Should().BeOfType<TicketClosed>();
    }

    [Fact]
    public void InvalidTicketData_ShouldThrowArgumentNullException()
    {
        // Given
        string title = string.Empty;
        string description = string.Empty;
        
        // When
        Func<Ticket> action = () =>
            Ticket.OpenNew(title, description, TicketPriority.LOW);

        // Then
        action.Should().Throw<ArgumentNullException>();
    }
}