using Oaf.Rabbit.Sdk;
using Newtonsoft.Json;
using Oaf.Rabbit.Sdk.Options;
using Newtonsoft.Json.Converters;
using Couchbase.Core.IO.Serializers;
using Microsoft.Extensions.Configuration;
using Couchbase.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using Infrastructure.Email;
using Infrastructure.Persistence;
using Application.Common.Interfaces;
using Infrastructure.RabbitMqEventBus;

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
                var serializer = new DefaultSerializer();

                serializer.SerializerSettings.Converters.Add(new StringEnumConverter());
                serializer.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                serializer.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                serializer.DeserializationSettings.Converters.Add(new StringEnumConverter());

                options.Serializer = serializer;
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