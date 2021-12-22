using Microsoft.Extensions.DependencyInjection;
using NewsTrack.Identity.Encryption;
using NewsTrack.Identity.Services;

namespace NewsTrack.Identity.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddIdentityDependencies(this IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ICryptoManager, CryptoManager>();
            return services;
        }
    }
}
