using Microsoft.Extensions.DependencyInjection;

namespace NewsTrack.Browser.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBrowserDependencies(this IServiceCollection services)
        {
            services.AddScoped<Browser.IRequestor, Browser.Requestor>();
            services.AddScoped<Browser.IBroswer, Browser.Broswer>();
            return services;
        }
    }
}
