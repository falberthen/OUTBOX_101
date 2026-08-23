namespace Outbox_101.EventConsumer.Configurations;

public static class ConsumerSetup
{
    public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, 
        IConfiguration configuration)
    {
        return services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()))
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