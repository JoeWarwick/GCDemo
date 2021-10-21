using GCDAPI.Services;
using GCDRepository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

[assembly: FunctionsStartup(typeof(GCDAPI.Startup))]
namespace GCDAPI
{
    public class Startup : FunctionsStartup
    {
        private string _appEnvPath;

        public Startup()
        {
            var binpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootpath = Path.GetFullPath(Path.Combine(binpath, ".."));
            _appEnvPath = rootpath;
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
