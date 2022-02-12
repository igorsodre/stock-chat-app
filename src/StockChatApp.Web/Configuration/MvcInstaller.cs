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
        if (environment.IsDevelopment())
        {
            var serverSettings = configuration.GetSection("ServerSettings").Get<ServerSettings>();
            services.AddCors(
                options => {
                    options.AddDefaultPolicy(
                        policyBuilder => {
                            policyBuilder.WithOrigins(serverSettings.SpaUrl)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        }
                    );
                }
            );
        }

        services.AddControllersWithViews();
    }
}