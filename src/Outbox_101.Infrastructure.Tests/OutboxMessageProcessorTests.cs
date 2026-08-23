namespace Outbox_101.Infrastructure.Tests;

public class OutboxMessageProcessorTests
{
    private readonly IOutboxMessages _outboxMessages = Substitute.For<IOutboxMessages>();
    private readonly IKafkaProducer _kafkaProducer = Substitute.For<IKafkaProducer>();
    private readonly ILogger<OutboxMessageProcessor> _logger = Substitute.For<ILogger<OutboxMessageProcessor>>();

    [Fact]
    public async Task FetchUnprocessedAsync_ShouldReturnOnlyUnprocessedMessages()
    {
        // Arrange
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

        _outboxMessages.FetchUnprocessedAsync(batchSize, CancellationToken.None)
            .Returns(outboxMessages);

        var unitOfWork = Substitute.For<ITicketUnitOfWork>();
        unitOfWork.OutboxMessages.Returns(_outboxMessages);

        IOptions<OutboxMessageProcessorOptions> options = Options.Create(
            new OutboxMessageProcessorOptions() { BatchSize = batchSize, Interval = TimeSpan.FromSeconds(10) });

        var messageProcessor = new OutboxMessageProcessor(
            unitOfWork,
            _kafkaProducer,
            options,
            _logger);

        // Act
        await messageProcessor.ProcessMessagesAsync(CancellationToken.None);

        var processedMessages = outboxMessages
            .Select(m => m.ProcessedAt is not null)
            .ToList();

        // Assert
        Assert.NotNull(processedMessages);
        processedMessages.Count().Should().Be(outboxMessages.Count());
    }
}
