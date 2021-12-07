using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class ConfirmIdentityEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/identity/confirm/{email}/{code}";

        public ConfirmIdentityEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenValidIdentityPendingToBeActivated_WhenConfirmingIdentity_ThenActivatesAccountAndRedirectsTo()
        {
            // Arrange
            const string securityCode = "some_code";
            const string redirectUrl = "http://www.url.com";
            
            var identity = Factory.Identity;
            identity.SecurityStamp = securityCode;
            identity.IsEnabled = false;
            
            var url = Endpoint.Replace("{email}", identity.Email).Replace("{code}", securityCode);
            var endpoint = GetUriWithQueryString(url, ("go", redirectUrl));

            Factory.IdentityRepositoryMock.Setup(m => m.GetByEmail(identity.Email)).Returns(Task.FromResult(identity));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeRedirectedTo(redirectUrl);
        }
    }
}
