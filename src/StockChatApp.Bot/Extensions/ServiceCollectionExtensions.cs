using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using StockChatApp.Bot.Consumers;
using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Options;
using StockChatApp.Bot.Producers;
using StockChatApp.Bot.Services;
using EventHandler = StockChatApp.Bot.Services.EventHandler;

namespace StockChatApp.Bot.Extensions;

public static class ServiceCollectionExtensions
{
    public static void InstallServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Stock Http client
        services.AddHttpClient(
            "stockApiClient",
            options => { options.BaseAddress = new Uri(configuration.GetValue<string>("StockApiSettings:BaseUrl")); }
        );

        // Dependency Injection
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
        services.Configure<HubSettings>(configuration.GetSection("HubSettings"));
        services.Configure<StockApiSettings>(configuration.GetSection("StockApiSettings"));
        services.AddScoped<ICsvStockParser, CsvStockParser>();
        services.AddScoped<IStockApiService, StockApiService>();
        services.AddScoped<IStockResultFormatter, StockResultFormatter>();
        services.AddSingleton<IEventHandler, EventHandler>();
        services.AddSingleton<StockProducer>();
        services.AddSingleton<IConnectionFactory>(
            provider => {
                var queueSettings = provider.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
                return new ConnectionFactory
                {
                    HostName = queueSettings.Host,
                    Port = queueSettings.Port,
                    UserName = queueSettings.UserName,
                    Password = queueSettings.Password
                };
            }
        );
        services.Scan(
            scan => {
                scan.FromAssemblyOf<ICommandProcessor>()
                    .AddClasses(classes => classes.AssignableTo<ICommandProcessor>())
                    .AsSelf()
                    .WithScopedLifetime();
            }
        );

        // BackgroundServices 
        services.AddHostedService<StockRequestConsumer>();
    }
}
