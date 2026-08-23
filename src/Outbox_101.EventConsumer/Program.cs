var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json", true, true)
    .Build();

Host.CreateDefaultBuilder(args)
     .ConfigureAppConfiguration(builder =>
     {
         builder.Sources.Clear();
         builder.AddConfiguration(configuration);
     })
    .ConfigureServices(services =>
    {
        services
            .AddPersistence(configuration)
            .AddKafkaConsumer(configuration);
    })
    .Build()
    .Run();
