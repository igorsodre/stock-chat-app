using StockChatApp.Bot.Interfaces;
using StockChatApp.Bot.Options;
using StockChatApp.Bot.Services;

var builder = WebApplication.CreateBuilder(args);

var stokApiSettings = new StockApiSettings();
builder.Configuration.GetSection("StockApiSettings").Bind(stokApiSettings);
builder.Services.AddHttpClient(
    "stockApiClient",
    options => { options.BaseAddress = new Uri(stokApiSettings.BaseUrl); }
);
builder.Services.AddScoped<ICsvStockParser, CsvStockParser>();
builder.Services.AddScoped<IStockApiService, StockApiService>();

var app = builder.Build();

app.Run();
