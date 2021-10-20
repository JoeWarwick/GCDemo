using GCDAPI.Services;
using GCDRepository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(GCDAPI.Startup))]
namespace GCDAPI
{
    public class Startup : FunctionsStartup
    {
        private string _appEnvPath;

        public Startup()
        {
            _appEnvPath = Environment.CurrentDirectory;
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
           
            builder.Services.AddDbContext<GCDModel>(opt => {
                opt.UseSqlite($"Data source={_appEnvPath}\\gcdapi.db");
            });

            builder.Services.AddScoped<IToDoService, ToDoService>();
                        
            var sp = builder.Services.BuildServiceProvider();
        }
    }
}
