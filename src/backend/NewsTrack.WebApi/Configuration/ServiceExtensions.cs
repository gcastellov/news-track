using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NewsTrack.Domain.Services;
using NewsTrack.Identity.Repositories;
using NewsTrack.Identity.Services;
using NewsTrack.WebApi.Components;
using NewsTrack.WebApi.Dtos.Profiles;
using NewsTrack.WebApi.HostedServices;

namespace NewsTrack.WebApi.Configuration
{
    internal static class ServiceExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(map => map.AddProfiles(new Profile[]
            {
                new BrowserProfile(),
                new DraftProfile(),
                new IdentityProfile(),
                new NewsProfile(),
                new CommentProfile()
            }));

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationProvider configuration)
        {
            services.AddSingleton<Configuration.IConfigurationProvider>(provider => configuration);
            return services;
        }

        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, ConfigurationProvider configuration)
        {
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
                    ValidIssuer = configuration.TokenConfiguration.Issuer,
                    ValidAudience = configuration.TokenConfiguration.Audience,
                    IssuerSigningKey = configuration.TokenConfiguration.SigningKey
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityPolicies.RequireAdministratorRole, policy => policy.RequireRole(IdentityRoles.Administrator));
            });

            return services;
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<SeederHostedService>();
            services.AddHostedService<SuggestionsHostedService>();
            return services;
        }

        public static IServiceCollection AddInternals(
            this IServiceCollection services,
            ConfigurationProvider configurationProvider,
            IConfigurationRoot configuration)
        {
            services.AddScoped<INotificator, Notificator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IIdentityHelper, IdentityHelper>();
            services.AddSingleton<Data.Configuration.IDataConfigurationProvider>(configurationProvider);
            services.AddScoped<ISeeder>(provider => new Seeder(
                provider.GetService<Data.Configuration.IDataInitializer>(),
                provider.GetService<IIdentityService>(),
                provider.GetService<IWebsiteService>(),
                provider.GetService<IIdentityRepository>(),
                configuration));

            return services;
        }
    }
}
