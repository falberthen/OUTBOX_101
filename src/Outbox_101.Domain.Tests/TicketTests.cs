using FluentAssertions;
using Outbox_101.Domain.Tickets;

namespace Outbox_101.Domain.Tests;

public class TicketTests
{
    [Fact]
    public void OpenNewTicket_ShouldSetOpenStatus()
    {
        // Given
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;

        // When
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // Then
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
        // Given
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;
        var ticket = Ticket.OpenNew(title, description, ticketPriority);

        // When
        ticket.SetInProgress();

        // Then
        Assert.NotNull(ticket);
        ticket.Status.Should().Be(TicketStatus.IN_PROGRESS);
    }

    [Fact]
    public void SetTicketClosed_ShouldHaveClosedStatus()
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
        ticket.Status.Should().Be(TicketStatus.CLOSED);
    }
}