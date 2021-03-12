using System;
using System.IO;
using Application;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sdk;
using sdk.Options;
using Serilog;

namespace WorldConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog((hostContext, config) => { config.ReadFrom.Configuration(hostContext.Configuration); })
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath($"{Directory.GetParent(Directory.GetCurrentDirectory()).FullName}/config");
                    configHost.AddJsonFile("appsettings.json", optional: true);
                    configHost.AddJsonFile("appsettings.Development.json", optional: true);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);

                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    if (environment == "Development")
                    {
                        configHost.AddUserSecrets<WorldConsumer>();
                    }
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    var configuration = hostBuilderContext.Configuration;
                    services.AddOafRabbit(options =>
                    {
                        var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

                        options.HostName = rabbitMqOptions.HostName;
                        options.Exchange = rabbitMqOptions.Exchange;
                        options.UserName = rabbitMqOptions.UserName;
                        options.Password = rabbitMqOptions.Password;
                        options.RoutingKeys = rabbitMqOptions.RoutingKeys;
                        options.Port = rabbitMqOptions.Port;
                        options.ConnectRetries = rabbitMqOptions.ConnectRetries;
                        options.ConnectRetriesTimeSpan = rabbitMqOptions.ConnectRetriesTimeSpan;
                    });

                    services.AddApplication();
                    services.AddInfrastructure(configuration);
                    services.AddHostedService<WorldConsumer>();
                })
                .UseConsoleLifetime();
        }
    }
}
