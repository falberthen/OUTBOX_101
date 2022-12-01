using Moq;
using Outbox_101.Domain.Tickets;
using Outbox_101.Infrastructure.Outbox;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Outbox_101.Infrastructure.Persistence;
using Outbox_101.Infrastructure.Kafka.Producers;
using Outbox_101.Infrastructure.Outbox.Polling;

namespace Outbox_101.Infrastructure.Tests;

public class OutboxMessageProcessorTests
{
    [Fact]
    public async Task FetchUnprocessedAsync_ShouldReturnOnlyUnprocessedMessages()
    {
        // Given
        int batchSize = 10;
        string title = "Title";
        string description = "Description";
        var ticketPriority = TicketPriority.MEDIUM;

        var tickets = new List<Ticket>();
        for (int i = 0; i < batchSize; i++)
            tickets.Add(Ticket.OpenNew(title, description, ticketPriority));

        IReadOnlyList<OutboxMessage> outboxMessages = tickets.SelectMany(ticket =>
        {
            return ticket.OutboxUncommitedEvents();
        }).ToArray();

        _outboxMessages.Setup(o => o.FetchUnprocessedAsync(batchSize, CancellationToken.None))
            .Returns(Task.FromResult(outboxMessages));

        var unitOfWork = new Mock<ITicketUnitOfWork>();
        unitOfWork.Setup(u => u.OutboxMessages)
            .Returns(_outboxMessages.Object);

        IOptions<OutboxMessageProcessorOptions> options = Options.Create(
            new OutboxMessageProcessorOptions() { BatchSize = batchSize, Interval = TimeSpan.FromSeconds(10) });

        var messageProcessor = new OutboxMessageProcessor(
            unitOfWork.Object, 
            _kafkaProducer.Object,
            options,
            _logger.Object);

        // When
        await messageProcessor.ProcessMessagesAsync(CancellationToken.None);

        var processedMessages = outboxMessages
            .Select(m => m.ProcessedAt is not null)
            .ToList();

        // Then
        Assert.NotNull(processedMessages);
        processedMessages.Count().Should().Be(outboxMessages.Count());
    }

    private Mock<IOutboxMessages> _outboxMessages = new Mock<IOutboxMessages>();
    private Mock<IKafkaProducer> _kafkaProducer = new Mock<IKafkaProducer>();
    private Mock<ILogger<OutboxMessageProcessor>> _logger = new Mock<ILogger<OutboxMessageProcessor>>();
}