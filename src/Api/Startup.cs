using System.Text.Json;
using Api.Filters;
using Application;
using FluentValidation.AspNetCore;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });
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
                endpoints.MapGet("/", async context =>
                {
                    if(!env.IsDevelopment())
                    {
                        context.Request.Path = Configuration.GetValue<string>("Oaf-NetCore-Starter-MW:BasePath");
                    }
                    var info = new
                    {
                        name = Configuration.GetValue<string>("Oaf-NetCore-Starter-MW:Name"),
                        version = Configuration.GetValue<string>("Oaf-NetCore-Starter-MW:Version"),
                        health = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}actuator/health",
                        documentation = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}swagger",
                    };
                    var infoJson = JsonSerializer.Serialize(info);
                    await context.Response.WriteAsync(infoJson);
                });
            });
        }
    }
}