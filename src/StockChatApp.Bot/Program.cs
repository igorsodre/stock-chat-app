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
var workerSettings = new WorkerSettings();
builder.Configuration.GetSection("WorkerSettings").Bind(workerSettings);
var rabbitMqSettings = new RabbitMqSettings();
builder.Configuration.GetSection("RabbitMqSettings").Bind(rabbitMqSettings);
var hubSettings = new HubSettings();
builder.Configuration.GetSection("HubSettings").Bind(hubSettings);
builder.Services.AddHostedService<StockRequestConsumer>();

// Dependency Injection
builder.Services.AddScoped<ICsvStockParser, CsvStockParser>();
builder.Services.AddScoped<IStockApiService, StockApiService>();
builder.Services.AddScoped<IStockResultFormatter, StockResultFormatter>();
builder.Services.AddSingleton(workerSettings);
builder.Services.AddSingleton(rabbitMqSettings);
builder.Services.AddSingleton(hubSettings);

var app = builder.Build();
app.Run();
