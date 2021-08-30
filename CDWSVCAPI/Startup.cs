using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using CDWRepository;
using Microsoft.EntityFrameworkCore;
using CDWSVCAPI.Caching;
using CDWSVCAPI.Services;
using Microsoft.AspNetCore.Identity;

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

            builder.Services.AddDbContext<CDWSVCModel<CDWSVCUser>>(opt => {
                opt.UseSqlite("Data source=cdwapi.db");
            });

            builder.Services.AddOptions<ConfigModel>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("Config").Bind(settings);
                });

            builder.Services.AddIdentity<CDWSVCUser, IdentityRole>()
                .AddEntityFrameworkStores<CDWSVCModel<CDWSVCUser>>()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<ISubscriptionsService, SubscriptionsService>();
            builder.Services.AddSingleton<IFeedService, FeedService>();

            builder.Services.AddSingleton<AutoFeedRefreshCache>()
                .AddSingleton<AutoImageRefreshCache>()
                .AddSingleton<AutoMetaRefreshCache>();

            var sp = builder.Services.BuildServiceProvider();
        }

    }
}
