using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NewsTrack.Data.Repositories;
using NewsTrack.Domain.Repositories;
using NewsTrack.Identity.Repositories;
using System.Linq;

namespace NewsTrack.WebApi.IntegrationTests.Fixture
{
    public class TestWebAppFactory<T> : WebApplicationFactory<T> where T : class
    {
        internal Mock<IContentRepository> ContentRepositoryMock { get; }
        internal Mock<IDraftRelationshipRepository> DraftRelationshipRepositoryMock { get; }
        internal Mock<IDraftRepository> DraftRepositoryMock { get; }
        internal Mock<IDraftSuggestionsRepository> DraftSuggestionsRepositoryMock { get; }
        internal Mock<IIdentityRepository> IdentityRepositoryMock { get; }
        internal Mock<IWebsiteRepository> WebsiteRepositoryMock { get; }

        public TestWebAppFactory()
        {
            ContentRepositoryMock = new Mock<IContentRepository>();
            DraftRelationshipRepositoryMock = new Mock<IDraftRelationshipRepository>();
            DraftRepositoryMock = new Mock<IDraftRepository>();
            DraftSuggestionsRepositoryMock = new Mock<IDraftSuggestionsRepository>();
            IdentityRepositoryMock = new Mock<IIdentityRepository>();
            WebsiteRepositoryMock = new Mock<IWebsiteRepository>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var repositories = services.Where(s => s.ServiceType.Namespace == typeof(IRepositoryBase).Namespace).ToArray();
                foreach (var repository in repositories)
                {
                    services.Remove(repository);
                }

                services.AddScoped(sp => ContentRepositoryMock.Object);
                services.AddScoped(sp => DraftRelationshipRepositoryMock.Object);
                services.AddScoped(sp => DraftRepositoryMock.Object);
                services.AddScoped(sp => DraftSuggestionsRepositoryMock.Object);
                services.AddScoped(sp => IdentityRepositoryMock.Object);
                services.AddScoped(sp => WebsiteRepositoryMock.Object);
            });
        }
    }
}
