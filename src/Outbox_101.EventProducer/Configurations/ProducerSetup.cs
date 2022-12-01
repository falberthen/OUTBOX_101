using Outbox_101.Infrastructure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Outbox_101.Infrastructure.Workers.Outbox.Polling;
using Outbox_101.Infrastructure.Workers.Outbox.Debezium;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Outbox_101.Infrastructure.Kafka.Producers;
using Outbox_101.Infrastructure.Outbox.Polling;
using Outbox_101.Infrastructure.Outbox;

namespace Outbox_101.EventConsumer.Configurations;

public static class ProducerSetup
{
    public static IServiceCollection AddOutboxSetup(this IServiceCollection services, 
        IConfiguration configuration, bool useDebezium = false)
    {
        if(useDebezium)
        {
            services.Configure<DebeziumSettings>(configuration.GetSection("DebeziumSettings"));
            services.TryAddSingleton<IDebeziumConnectorSetup, DebeziumConnectorSetup>();
            return services
                .AddHostedService(serviceProvider =>
                {
                    var consumer = serviceProvider.GetRequiredService<IDebeziumConnectorSetup>();
                    return new BackgroundWorker(consumer.StartConfiguringAsync);
                });
        }

        return services.AddScoped<IKafkaProducer, KafkaProducer>()            
            .AddScoped<IOutboxMessageProcessor, OutboxMessageProcessor>()
            .AddScoped<IOutboxMessageProcessingWorker, OutboxMessageProcessingWorker>()
            .AddHostedService(serviceProvider =>
            {
                var consumer = serviceProvider.GetRequiredService<IOutboxMessageProcessingWorker>();
                return new BackgroundWorker(consumer.StartProcessingAsync);
            })
            .Configure<KafkaProducerOptions>(configuration.GetSection("KafkaProducer"))
            .Configure<OutboxMessageProcessorOptions>(configuration.GetSection("OutboxMessageProcessorOptions"));
    }
}
