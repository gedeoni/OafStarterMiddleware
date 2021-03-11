using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    builder.SetBasePath($"{Directory.GetParent(Directory.GetCurrentDirectory()).FullName}/config");
                    builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        // builder.AddUserSecrets<Startup>();
                        builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
