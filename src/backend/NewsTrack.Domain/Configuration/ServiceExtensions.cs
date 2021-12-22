using Microsoft.Extensions.DependencyInjection;
using NewsTrack.Domain.Services;

namespace NewsTrack.Domain.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
        {
            services.AddScoped<IDraftService, DraftService>();
            services.AddScoped<IWebsiteService, WebsiteService>();
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<ICommentService, CommentService>();
            return services;
        }
    }
}
