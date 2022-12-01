using Microsoft.Extensions.Hosting;

namespace Outbox_101.Infrastructure.Workers;

public class BackgroundWorker : BackgroundService
{
    private readonly Func<CancellationToken, Task> _perform;

    public BackgroundWorker(Func<CancellationToken, Task> perform)
    {
        _perform = perform;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Run(async () =>
        {
            await Task.Yield();
            await _perform(stoppingToken);
        }, stoppingToken);
}