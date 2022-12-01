using Polly;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Outbox_101.Infrastructure.Outbox.Polling;
using Outbox_101.Infrastructure.Outbox;

namespace Outbox_101.Infrastructure.Workers.Outbox.Polling;

public class OutboxMessageProcessingWorker : IOutboxMessageProcessingWorker
{
    private readonly IOutboxMessageProcessor _outboxMessageProcessor;
    private readonly ILogger<OutboxMessageProcessingWorker> _logger;
    private readonly OutboxMessageProcessorOptions _processorOptions;

    public OutboxMessageProcessingWorker(
        IOutboxMessageProcessor outboxMessageProcessor,
        IOptions<OutboxMessageProcessorOptions> options,
        ILogger<OutboxMessageProcessingWorker> logger)
    {
        _outboxMessageProcessor = outboxMessageProcessor;
        _processorOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task StartProcessingAsync(CancellationToken cancellationToken = default)
    {
        var policy = Policy
           .Handle<Exception>()
           .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        await policy.ExecuteAsync(
           async () =>
           {
               _logger.LogInformation("Starting outbox message processor...");

               while (!cancellationToken.IsCancellationRequested)
                   await _outboxMessageProcessor.ProcessMessagesAsync(cancellationToken);

               await Task.Delay(_processorOptions.Interval, cancellationToken);
           });
    }
}