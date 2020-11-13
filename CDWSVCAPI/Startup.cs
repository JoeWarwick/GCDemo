using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using CDWRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CDWSVCAPI.Caching;
using CDWSVCAPI.Services;

[assembly: FunctionsStartup(typeof(CDWSVCAPI.Startup))]
namespace CDWSVCAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CDWSVCAPI", Version = "v1" });
            });

            builder.Services.AddOptions<ConfigModel>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("Config").Bind(settings);
                });

            builder.Services.AddScoped<ISubscriptionsService>();

            builder.Services.AddSingleton<AutoFeedRefreshCache>()
                .AddSingleton<AutoImageRefreshCache>()
                .AddSingleton<AutoMetaRefreshCache>();

            builder.Services.BuildServiceProvider();
        }

    }
}
