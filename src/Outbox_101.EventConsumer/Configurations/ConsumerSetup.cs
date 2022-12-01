using MediatR;
using Outbox_101.Infrastructure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Outbox_101.Infrastructure.Kafka.Consumers;
using Outbox_101.Infrastructure.Kafka.Consumers.Serialization;

namespace Outbox_101.EventConsumer.Configurations;

public static class ConsumerSetup
{
    public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, 
        IConfiguration configuration)
    {
        return services
            .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
            .AddScoped<IKafkaConsumer, KafkaConsumer>()
            .AddSingleton<IEventDispatcher, EventDispatcher>()
            .AddSingleton(typeof(JsonEventSerializer<>))
            .AddHostedService(serviceProvider =>
            {
                var consumer = serviceProvider.GetRequiredService<IKafkaConsumer>();
                return new BackgroundWorker(consumer.StartConsumeAsync);
            })
            .Configure<KafkaConsumerOptions>(configuration.GetSection("KafkaConsumer"));
    }
}