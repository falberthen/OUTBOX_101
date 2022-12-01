using Polly;
using System.Text;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Outbox_101.Infrastructure.Workers.Outbox.Debezium;

public class DebeziumConnectorSetup : IDebeziumConnectorSetup
{
    private readonly DebeziumSettings _debeziumSettings;
    private IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DebeziumConnectorSetup> _logger;

    public DebeziumConnectorSetup(
        IOptions<DebeziumSettings> debeziumSettings,
        IHttpClientFactory httpClientFactory,
        ILogger<DebeziumConnectorSetup> logger)
    {
        _debeziumSettings = debeziumSettings.Value ?? throw new ArgumentNullException(nameof(debeziumSettings));
        _httpClientFactory = httpClientFactory;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task StartConfiguringAsync(CancellationToken cancellationToken = default)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        await retryPolicy.ExecuteAsync(
            async () =>
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var fileName = "debezium-ticket-connector.json";
                var response = await httpClient.PutAsync(
                    _debeziumSettings.ConnectorUrl,
                    new StringContent(
                        File.ReadAllText(fileName),
                        Encoding.UTF8,
                        MediaTypeNames.Application.Json));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Was not possible to configure Debezium. Status code: {StatusCode}", response.StatusCode);
                    throw new Exception("Was not possible to configure Debezium outbox.");
                }
            });
    }
}
