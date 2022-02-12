using StockChatApp.Web.Configuration;

namespace StockChatApp.Web.Extensions.Configuration;

internal static class ConfigurationExtensions
{
    public static void InstallServicesInAssembly(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        var configurationInstallers = typeof(Program).Assembly.ExportedTypes
            .Where((x) => typeof(IServiceInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>()
            .ToList();

        foreach (var installer in configurationInstallers)
        {
            installer.InstallServices(services, configuration, environment);
        }
    }
}
