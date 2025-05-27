using CodeBlock.PlayerSample.Configuration;
using CodeBlock.PlayerSample.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((context, config) =>
    {
        // Add configuration sources
        if (!context.HostingEnvironment.IsProduction())
        {
            config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        }
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<GameEngineConfiguration>(context.Configuration.GetSection("GameEngineConfiguration"));
        services.AddSingleton<GamePersistence>();
        services.AddLogging();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddHttpClient();
        
        services.Configure<BotConfiguration>(options =>
        {
            // Bind from configuration first
            context.Configuration.GetSection("BotConfiguration").Bind(options);
            
            // Then set runtime values
            var hostname = context.Configuration["WEBSITE_HOSTNAME"];
            options.BaseUrl = string.IsNullOrEmpty(hostname) 
                ? "http://localhost:7071" 
                : $"http://{hostname}";
        });
    })
    .Build();

host.Run();