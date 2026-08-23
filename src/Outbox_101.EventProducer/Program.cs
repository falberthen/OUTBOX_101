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