using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.Domain.Entities;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class NewsRelationshipEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/news/entry/{id}/relationship";

        public NewsRelationshipEndpointTests(TestWebAppFactory<Program> testWebAppFactory)
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingRelationship_WhenGettingIt_ThenReturnsIt()
        {
            // Arrange
            var relationshipItem = new DraftRelationshipItem
            {
                Id = Guid.NewGuid(),
                Title = "First title",
                Url = new Uri("http://www.some.com/path/resource")
            };

            var draftRelationship = new DraftRelationship
            {
                Id = Guid.NewGuid(),
                Relationship = new[] { relationshipItem }
            };

            string endpoint = Endpoint.Replace("{id}", draftRelationship.Id.ToString());

            Factory.DraftRelationshipRepositoryMock.Setup(m => m.Get(draftRelationship.Id)).Returns(Task.FromResult(draftRelationship));

            // Act
            var response = await Client.GetAsync(endpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<IEnumerable<NewsDigestDto>>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Should().HaveCount(draftRelationship.Relationship.Count());
            
            var item = envelope.Payload.First();
            item.Id.Should().Be(relationshipItem.Id);
            item.Title.Should().Be(relationshipItem.Title);
            item.Url.Should().Be(relationshipItem.Url);
        }
    }
}
