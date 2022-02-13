using StockChatApp.Web.Contracts.Producers;
using StockChatApp.Web.Interfaces;
using StockChatApp.Web.Options;
using StockChatApp.Web.Producers;

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

        services.AddSingleton(rabbitMqSettings);
        services.AddSingleton<IProducer<CommandDto<StockRequestDto>>, StockRequestProducer>();
    }
}
