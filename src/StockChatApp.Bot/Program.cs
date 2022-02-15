using StockChatApp.Bot.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallServices(builder.Configuration);

var app = builder.Build();
app.Run();
