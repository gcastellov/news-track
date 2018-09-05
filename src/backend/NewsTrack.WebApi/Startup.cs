using System;
using System.IO;
using System.Net;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsTrack.Domain.Repositories;
using NewsTrack.Domain.Services;
using NewsTrack.Identity.Encryption;
using NewsTrack.Identity.Repositories;
using NewsTrack.Identity.Services;
using NewsTrack.WebApi.Components;
using ConfigurationProvider = NewsTrack.WebApi.Configuration.ConfigurationProvider;
using IConfigurationProvider = NewsTrack.Data.Configuration.IConfigurationProvider;

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
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Add AutoMapper
            services.AddAutoMapper();

            // Add framework services.
            services.AddMvc();
            services.AddCors();

            // Add authentication
            var configurationProvider = new ConfigurationProvider();
            configurationProvider.Set(Configuration);
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
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configurationProvider.TokenConfiguration.Issuer,
                        ValidAudience = configurationProvider.TokenConfiguration.Audience,
                        IssuerSigningKey = configurationProvider.TokenConfiguration.SigningKey
                    };
                });

            // Create the container builder.
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<Browser.Requestor>().As<Browser.IRequestor>();
            builder.RegisterType<Browser.Broswer>().As<Browser.IBroswer>();
            builder.RegisterType<DraftService>().As<IDraftService>().InstancePerLifetimeScope();
            builder.RegisterType<WebsiteService>().As<IWebsiteService>().InstancePerLifetimeScope();
            builder.RegisterType<ContentService>().As<IContentService>().InstancePerLifetimeScope();
            builder.Register(c =>
            {
                var notificationManager = new NotificationManager(configurationProvider);
                return new IdentityService(
                    c.Resolve<IIdentityRepository>(),
                    c.Resolve<ICryptoManager>(),
                    notificationManager.Handle
                );
            }).As<IIdentityService>().InstancePerLifetimeScope();
            builder.RegisterType<IdentityHelper>().As<IIdentityHelper>().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            builder.RegisterType<CryptoManager>().As<ICryptoManager>();
            builder.RegisterType<Data.Repositories.ContentRepository>().As<IContentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<Data.Repositories.DraftRepository>().As<IDraftRepository>().InstancePerLifetimeScope();
            builder.RegisterType<Data.Repositories.DraftRelationshipRepository>().As<IDraftRelationshipRepository>().InstancePerLifetimeScope();
            builder.RegisterType<Data.Repositories.DraftSuggestionsRepository>().As<IDraftSuggestionsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<Data.Repositories.IdentityRepository>().As<IIdentityRepository>().InstancePerLifetimeScope();
            builder.RegisterType<Data.Repositories.WebsiteRepository>().As<IWebsiteRepository>().InstancePerLifetimeScope();
            builder.RegisterType<Data.Repositories.ContentRepository>().As<Data.Repositories.IRepositoryBase>();
            builder.RegisterType<Data.Repositories.DraftRepository>().As<Data.Repositories.IRepositoryBase>();
            builder.RegisterType<Data.Repositories.DraftRelationshipRepository>().As<Data.Repositories.IRepositoryBase>();
            builder.RegisterType<Data.Repositories.IdentityRepository>().As<Data.Repositories.IRepositoryBase>();
            builder.RegisterType<Data.Repositories.DraftSuggestionsRepository>().As<Data.Repositories.IRepositoryBase>();
            builder.RegisterType<Data.Repositories.WebsiteRepository>().As<Data.Repositories.IRepositoryBase>();
            builder.RegisterType<Data.Configuration.DataInitializer>().As<Data.Configuration.IDataInitializer>();
            builder.Register(c => new Data.Configuration.ConfigurationProvider { ConnectionString = Configuration.GetConnectionString("ElasticSearch")})
                .As<IConfigurationProvider>().SingleInstance();
            builder.Register(c => configurationProvider).As<Configuration.IConfigurationProvider>().SingleInstance();
            builder.Register(c => new Seeder(
                    c.Resolve<Data.Configuration.IDataInitializer>(),
                    c.Resolve<IIdentityService>(),
                    c.Resolve<IWebsiteService>(),
                    c.Resolve<IIdentityRepository>(),
                    Configuration
                ))
                .As<ISeeder>();
            ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(
                options =>
                {
                    options.Run(
                        async context =>
                        {
                            var message = "An unhandled exception has been thrown";
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "text/plain";
                            context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                            var ex = context.Features.Get<IExceptionHandlerFeature>();
                            if (ex != null)
                            {
                                var logger = loggerFactory.CreateLogger<Startup>();
                                logger.LogError(ex.Error, message);
                            }

                            await context.Response.WriteAsync(message).ConfigureAwait(false);
                        });
                }
            );

            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync("Status code page, status code: " + context.HttpContext.Response.StatusCode)
                    .ConfigureAwait(false);
            });

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication();
            app.UseMvc();

            try
            {
                var seeder = ApplicationContainer.Resolve<ISeeder>();
                seeder.Initialize();
            }
            catch(Exception e)
            {
                var logger = loggerFactory.CreateLogger<Startup>();
                logger.LogError(e, "An exception has been thrown whilst trying to set default data");
            }
        }
    }
}
