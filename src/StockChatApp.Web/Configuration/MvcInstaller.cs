using StockChatApp.Web.Options;

namespace StockChatApp.Web.Configuration;

public class MvcInstaller : IServiceInstaller
{
    public void InstallServices(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        var serverSettings = configuration.GetSection("ServerSettings").Get<ServerSettings>();
        if (environment.IsDevelopment())
        {
            services.AddCors(
                options => {
                    options.AddDefaultPolicy(
                        policyBuilder => {
                            policyBuilder.WithOrigins(serverSettings.AllowedUrlOrigins)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        }
                    );
                }
            );
        }

        services.AddSingleton(serverSettings);
        services.AddControllers();
        services.AddControllersWithViews();
        services.AddSignalR();
    }
}
