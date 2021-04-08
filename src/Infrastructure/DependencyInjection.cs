using Application.Common.Interfaces;
using Couchbase.Extensions.DependencyInjection;
using Infrastructure.Email;
using Infrastructure.Persistence;
using Infrastructure.RabbitMqEventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oaf.Rabbit.Sdk;
using Oaf.Rabbit.Sdk.Options;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration Configuration)
        {
            var couchbaseOptions = Configuration.GetSection("Couchbase").Get<CouchbaseConfig>();

            services.AddCouchbase(options => {
                options.EnableTls = false;
                options.ConnectionString = Configuration.GetConnectionString("couchbase:data");
                options.WithCredentials(couchbaseOptions.Username, couchbaseOptions.Password);
            });

            services.AddCouchbaseBucket<IWorldsBucket>(couchbaseOptions.BucketName);
            services.AddSingleton<ICouchbaseContext, CouchbaseContext>();
            services.AddSingleton<IWorldRepository, WorldRepository>();
            services.AddSingleton<ISendEmails, FakeEmailClient>();
            services.AddSingleton<IPublishEvent, RabbitMQEventBus>();
            services.AddOafRabbit(options => {
                var rabbitMqOptions = Configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();

                options.HostName = rabbitMqOptions.HostName;
                options.Exchange = rabbitMqOptions.Exchange;
                options.UserName = rabbitMqOptions.UserName;
                options.Password = rabbitMqOptions.Password;
                options.RoutingKeys = rabbitMqOptions.RoutingKeys;
                options.Port = rabbitMqOptions.Port;
                options.ConnectionRetries = rabbitMqOptions.ConnectionRetries;
                options.ConnectionRetriesTimeSpan = rabbitMqOptions.ConnectionRetriesTimeSpan;
            });
            return services;
        }
    }
}