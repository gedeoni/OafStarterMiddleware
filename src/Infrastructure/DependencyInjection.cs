using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Couchbase.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Http;
using sdk.Options;
using sdk;
using Infrastructure.RabbitMqEventBus;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration Configuration)
        {
            var options = Configuration.GetSection("Couchbase").Get<CouchbaseConfig>();

            services.AddCouchbase(repayments =>
            {
                repayments.EnableTls = false;
                repayments.ConnectionString = Configuration.GetConnectionString("couchbase:data");
                repayments.WithCredentials(options.Username, options.Password);
            });

            services.AddCouchbaseBucket<IRepaymentsBucket>(options.BucketName);
            services.AddSingleton<ICouchbaseContext, CouchbaseContext>();
            services.AddSingleton<IWorldRepository, WorldRepository>();
            services.AddSingleton<ISendEmails, SMTPEmailClient>();
            services.AddOafRabbit(options =>
            {
                var rabbitMqOptions = Configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

                options.HostName = rabbitMqOptions.HostName;
                options.Exchange = rabbitMqOptions.Exchange;
                options.UserName = rabbitMqOptions.UserName;
                options.Password = rabbitMqOptions.Password;
                options.RoutingKeys = rabbitMqOptions.RoutingKeys;
                options.Port = rabbitMqOptions.Port;
                options.ConnectRetries = rabbitMqOptions.ConnectRetries;
                options.ConnectRetriesTimeSpan = rabbitMqOptions.ConnectRetriesTimeSpan;
            });
            services.AddSingleton<IPublishEvent, RabbitMQEventBus>();
            return services;
        }
    }

    public interface IRepaymentsBucket : INamedBucketProvider { }
    public class CouchbaseConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] Servers { get; set; }
        public bool UseSsl { get; set; }
        public string BucketName { get; set; }
    }
}