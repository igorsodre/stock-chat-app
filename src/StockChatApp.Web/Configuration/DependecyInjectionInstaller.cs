using RabbitMQ.Client;
using StackExchange.Redis;
using StockChatApp.Web.Interfaces;
using StockChatApp.Web.Options;
using StockChatApp.Web.Services;

namespace StockChatApp.Web.Configuration;

public class DependecyInjectionInstaller : IServiceInstaller
{
    public void InstallServices(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        var rabbitMqSettings = new RabbitMqSettings();
        configuration.GetSection("RabbitMqSettings").Bind(rabbitMqSettings);

        var redisSettings = new RedisSettings();
        configuration.GetSection("RedisSettings").Bind(redisSettings);

        services.AddSingleton(rabbitMqSettings);
        services.AddSingleton(redisSettings);
        services.AddSingleton<IConnectionFactory>(
            provider => {
                var queueSettings = provider.GetRequiredService<RabbitMqSettings>();
                return new ConnectionFactory
                {
                    HostName = queueSettings.Host,
                    Port = queueSettings.Port,
                    UserName = queueSettings.UserName,
                    Password = queueSettings.Password
                };
            }
        );
        services.AddSingleton<IConnectionMultiplexer>(
            x => ConnectionMultiplexer.Connect(redisSettings.ConnectionString,
                options => {
                    options.ConnectRetry = 10;
                    options.AbortOnConnectFail = false;
                })
        );

        services.AddSingleton<IChatMessageRepository, ChatMessagesRepository>();

        services.Scan(
            scan => {
                scan.FromAssemblyOf<Program>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IProducer)))
                    .AsSelf()
                    .WithScopedLifetime();
            }
        );

        services.AddScoped<ICommandProcessor, CommandProcessor>();
    }
}
