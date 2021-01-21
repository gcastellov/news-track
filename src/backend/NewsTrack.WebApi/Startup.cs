using System;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NewsTrack.Domain.Repositories;
using NewsTrack.Domain.Services;
using NewsTrack.Identity.Encryption;
using NewsTrack.Identity.Repositories;
using NewsTrack.Identity.Services;
using NewsTrack.WebApi.Components;
using NewsTrack.WebApi.Configuration;
using NewsTrack.WebApi.Dtos.Profiles;

namespace NewsTrack.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
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
            //Add AutoMapper
            services.AddAutoMapper(map => map.AddProfiles(new Profile[]
            {
                new BrowserProfile(),
                new DraftProfile(),
                new IdentityProfile(),
                new NewsProfile()
            }));

            // Add authentication
            var configurationProvider = new Configuration.ConfigurationProvider();
            configurationProvider.Set(Configuration);

            services.AddScoped<Configuration.IConfigurationProvider>(provider => configurationProvider);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configurationProvider.TokenConfiguration.Issuer,
                    ValidAudience = configurationProvider.TokenConfiguration.Audience,
                    IssuerSigningKey = configurationProvider.TokenConfiguration.SigningKey
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityPolicies.RequireAdministratorRole, policy => policy.RequireRole(IdentityRoles.Administrator));
            });

            services.AddHostedService<SeederHostedService>();

            services.AddScoped<Browser.IRequestor, Browser.Requestor>();
            services.AddScoped<Browser.IBroswer, Browser.Broswer>();
            services.AddScoped<IDraftService, DraftService>();
            services.AddScoped<IWebsiteService, WebsiteService>();
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IIdentityService>(provider =>
            {
                var notificationManager = new NotificationManager(configurationProvider);
                return new IdentityService(
                    provider.GetService<IIdentityRepository>(),
                    provider.GetService<ICryptoManager>(),
                    notificationManager.Handle);
            });

            services.AddScoped<IIdentityHelper, IdentityHelper>();
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICryptoManager, CryptoManager>();
            services.AddScoped<IContentRepository, Data.Repositories.ContentRepository>();
            services.AddScoped<IDraftRepository, Data.Repositories.DraftRepository>();
            services.AddScoped<IDraftRelationshipRepository, Data.Repositories.DraftRelationshipRepository>();
            services.AddScoped<IDraftSuggestionsRepository, Data.Repositories.DraftSuggestionsRepository>();
            services.AddScoped<IIdentityRepository, Data.Repositories.IdentityRepository>();
            services.AddScoped<IWebsiteRepository, Data.Repositories.WebsiteRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.ContentRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.DraftRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.DraftRelationshipRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.IdentityRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.DraftSuggestionsRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.WebsiteRepository>();
            services.AddScoped<Data.Configuration.IDataInitializer, Data.Configuration.DataInitializer>();
            services.AddSingleton<Data.Configuration.IDataConfigurationProvider>(configurationProvider);

            services.AddScoped<ISeeder>(provider => new Seeder(
                    provider.GetService<Data.Configuration.IDataInitializer>(),
                    provider.GetService<IIdentityService>(),
                    provider.GetService<IWebsiteService>(),
                    provider.GetService<IIdentityRepository>(),
                    Configuration
                ));

            services.AddControllers();
            services.AddCors();
            services
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
