using System;
using System.IO;
using Hangfire;
using Hangfire.MemoryStorage;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsTrack.Browser.Configuration;
using NewsTrack.Data.Configuration;
using NewsTrack.Domain.Configuration;
using NewsTrack.Identity.Configuration;
using NewsTrack.WebApi;
using NewsTrack.WebApi.Configuration;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

builder.Host.ConfigureAppConfiguration((context, config) =>
{
    config
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();
})
.ConfigureLogging(logging =>
 {
     logging
        .ClearProviders()
        .SetMinimumLevel(LogLevel.Trace);
 })
.UseNLog();

services.AddControllers();
var configurationProvider = new NewsTrack.WebApi.Configuration.ConfigurationProvider();
configurationProvider.Set(builder.Configuration);

services
    .AddMediatR(typeof(Program))
    .AddAutoMapper()
    .AddConfiguration(configurationProvider)
    .AddAuthenticationAndAuthorization(configurationProvider)
    .AddHostedServices()
    .AddInternals(configurationProvider, builder.Configuration)
    .AddDomainDependencies()
    .AddDataDependencies()
    .AddIdentityDependencies()
    .AddBrowserDependencies()
    .AddCors()
    .AddHangfire(options => options.UseMemoryStorage())
    .AddHangfireServer()
    .AddHealthChecks()
    .AddElasticsearch(configurationProvider.ConnectionString.AbsoluteUri);

var app = builder.Build();
app
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


app.MapControllers();

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
logger.Info("Initializing application...");

try
{
    await app.RunAsync();
}
catch (Exception e)
{
    logger.Fatal(e, "The application has crashed. Check this out.");
}
finally
{
    NLog.LogManager.Shutdown();
}

public partial class Program { }
