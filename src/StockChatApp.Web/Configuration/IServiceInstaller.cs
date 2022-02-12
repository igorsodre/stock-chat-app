namespace StockChatApp.Web.Configuration;

public interface IServiceInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment);
}
