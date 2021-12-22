using Microsoft.Extensions.DependencyInjection;
using NewsTrack.Domain.Repositories;
using NewsTrack.Identity.Repositories;

namespace NewsTrack.Data.Configuration
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services)
        {
            services.AddScoped<IContentRepository, Data.Repositories.ContentRepository>();
            services.AddScoped<IDraftRepository, Data.Repositories.DraftRepository>();
            services.AddScoped<IDraftRelationshipRepository, Data.Repositories.DraftRelationshipRepository>();
            services.AddScoped<IDraftSuggestionsRepository, Data.Repositories.DraftSuggestionsRepository>();
            services.AddScoped<IWebsiteRepository, Data.Repositories.WebsiteRepository>();
            services.AddScoped<ICommentRepository, Data.Repositories.CommentRepository>();
            services.AddScoped<IIdentityRepository, Data.Repositories.IdentityRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.ContentRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.DraftRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.DraftRelationshipRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.IdentityRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.DraftSuggestionsRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.CommentRepository>();
            services.AddScoped<Data.Repositories.IRepositoryBase, Data.Repositories.WebsiteRepository>();
            services.AddScoped<Data.Configuration.IDataInitializer, Data.Configuration.DataInitializer>();
            return services;
        }
    }
}
