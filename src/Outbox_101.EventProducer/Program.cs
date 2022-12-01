using Outbox_101.EventProducer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Outbox_101.EventConsumer.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Outbox_101.Infrastructure.Persistence.Configurations;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json", true, true)
    .Build();

var host = Host.CreateDefaultBuilder(args)
     .ConfigureAppConfiguration(builder =>
     {
         builder.Sources.Clear();
         builder.AddConfiguration(configuration);
     })
     .ConfigureServices(services =>
     {
         services
             .AddHttpClient()
             .AddPersistence(configuration)
             .AddOutboxSetup(configuration);
     })
    .Build();

// Generating tickets
await TicketBuilder.Build(host);
await host.RunAsync();