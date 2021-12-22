using System.IO;
using AutoMapper;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsTrack.Browser.Configuration;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Configuration;
using NewsTrack.Identity.Configuration;
using NewsTrack.WebApi.Configuration;

namespace NewsTrack.WebApi
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var configurationProvider = new Configuration.ConfigurationProvider();
            configurationProvider.Set(Configuration);

            services
                .AddMediatR(typeof(Startup))
                .AddAutoMapper()
                .AddConfiguration(configurationProvider)
                .AddAuthenticationAndAuthorization(configurationProvider)
                .AddHostedServices()
                .AddInternals(configurationProvider, Configuration)
                .AddDomainDependencies()
                .AddDataDependencies()
                .AddIdentityDependencies()
                .AddBrowserDependencies()
                .AddCors()
                .AddHangfire(options => options.UseMemoryStorage())
                .AddHangfireServer()
                .AddHealthChecks()
                .AddElasticsearch(configurationProvider.ConnectionString.AbsoluteUri);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _ = app
                .UseMiddleware<ExceptionMiddleware>()
                .UseHttpsRedirection()
                .UseRouting()
                .UseCors(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                })
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/health");
                })
                .UseStatusCodePages(async context =>
                {
                    context.HttpContext.Response.ContentType = "text/plain";
                    await context.HttpContext.Response.WriteAsync("Status code page, status code: " + context.HttpContext.Response.StatusCode)
                        .ConfigureAwait(false);
                });
        }
    }
}
