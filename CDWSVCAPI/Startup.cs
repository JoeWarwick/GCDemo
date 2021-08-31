using CDWRepository;
using CDWSVCAPI.Caching;
using CDWSVCAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CDWSVCAPI.Startup))]
namespace CDWSVCAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
           
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
