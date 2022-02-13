using RabbitMQ.Client;
using StockChatApp.Bot.Consumers;
using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Options;
using StockChatApp.Bot.Services;

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
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddSingleton(rabbitMqSettings);
builder.Services.AddSingleton(hubSettings);
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

var app = builder.Build();
app.Run();
