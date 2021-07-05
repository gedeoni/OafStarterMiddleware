using System.Text.Json;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Api.Filters;
using Api.HealthChecks;

using Application;

using FluentValidation.AspNetCore;

using HealthChecks.UI.Client;

using Infrastructure;
using System.Collections.Generic;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddControllers(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
                    .AddFluentValidation();

            services
                .AddHealthChecks()
                .AddCouchbaseHealthCheck(
                    couchbaseConnectionString: Configuration.GetConnectionString("couchbase:data"),
                    username: Configuration["Couchbase:UserName"],
                    password: Configuration["Couchbase:Password"],
                    failureStatus: HealthStatus.Degraded,
                    tags: new List<string>(){"Database", "Couchbase"}
                )
                .AddRabbitMQ(
                    rabbitConnectionString: $"amqp://{Configuration["RabbitMQ:UserName"]}:{Configuration["RabbitMQ:Password"]}@{Configuration["RabbitMQ:HostName"]}:{Configuration["RabbitMQ:Port"]}/",
                    failureStatus: HealthStatus.Degraded,
                    tags: new List<string>(){"EventBus", "RabbitMQ"}
                );

            services.AddHealthChecksUI().AddInMemoryStorage();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("health", new HealthCheckOptions(){
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecksUI();

                endpoints.MapGet("/", async context =>
                {
                    var url = $"{context.Request.Scheme}://{context.Request.Host}";

                    if(!env.IsDevelopment())
                    {
                        context.Request.Path = Configuration.GetValue<string>("Oaf-NetCore-Starter-MW:BasePath");
                    }

                    var info = new
                    {
                        Name = Configuration["App:Name"],
                        Version = Configuration["App:Version"],
                        HealthChecks =  new {
                            Health = $"{url}/health",
                            HealthUI = $"{url}/healthchecks-ui#/healthchecks"
                        },
                        Documentation = $"{url}/swagger/index.html",
                    };

                    var infoJson = JsonSerializer.Serialize(info);

                    context.Response.Headers.Add("Content-Type", "application/json");
                    await context.Response.WriteAsync(infoJson);
                });
            });
        }
    }
}