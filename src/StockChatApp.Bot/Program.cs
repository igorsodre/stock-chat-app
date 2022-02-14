using RabbitMQ.Client;
using StockChatApp.Bot.Consumers;
using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Options;
using StockChatApp.Bot.Producers;
using StockChatApp.Bot.Services;
using EventHandler = StockChatApp.Bot.Services.EventHandler;

var builder = WebApplication.CreateBuilder(args);

// Stock Http client
var stokApiSettings = new StockApiSettings();
builder.Configuration.GetSection("StockApiSettings").Bind(stokApiSettings);
builder.Services.AddHttpClient(
    "stockApiClient",
    options => { options.BaseAddress = new Uri(stokApiSettings.BaseUrl); }
);

// BackgroundService
var rabbitMqSettings = new RabbitMqSettings();
builder.Configuration.GetSection("RabbitMqSettings").Bind(rabbitMqSettings);
builder.Services.AddHostedService<StockRequestConsumer>();

// Dependency Injection
var hubSettings = new HubSettings();
builder.Configuration.GetSection("HubSettings").Bind(hubSettings);
builder.Services.AddScoped<ICsvStockParser, CsvStockParser>();
builder.Services.AddScoped<IStockApiService, StockApiService>();
builder.Services.AddScoped<IStockResultFormatter, StockResultFormatter>();
builder.Services.AddSingleton<IEventHandler, EventHandler>();
builder.Services.AddSingleton(rabbitMqSettings);
builder.Services.AddSingleton(hubSettings);
builder.Services.AddSingleton<StockProducer>();
builder.Services.AddSingleton<IConnectionFactory>(
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
builder.Services.Scan(
    scan => {
        scan.FromAssemblyOf<ICommandProcessor>()
            .AddClasses(classes => classes.AssignableTo<ICommandProcessor>())
            .AsSelf()
            .WithScopedLifetime();
    }
);

var app = builder.Build();
app.Run();
