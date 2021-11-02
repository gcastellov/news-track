using FluentAssertions;
using NewsTrack.Domain.Exceptions;
using NewsTrack.WebApi.Dtos;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class TagStatsEndpointTests : BaseTest
    {
        private const string TagStatsEndpoint = "/api/tags/stats";

        public TagStatsEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenDifferentTags_WhenGettingStats_ThenReturnsScoreByTags()
        {
            // Arrange
            IDictionary<string, long> stats = new Dictionary<string, long>
            {
                { "tag1", 5 },
                { "tag2", 3 },
                { "tag3", 7 }
            };

            Factory.DraftRepositoryMock.Setup(m => m.GetTagsStats()).Returns(Task.FromResult(stats));

            // Act
            var response = await Client.GetAsync(TagStatsEndpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<TagsStatsResponseDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.MaxScore.Should().Be(7);
            envelope.Payload.AverageScore.Should().Be(5);
            envelope.Payload.Count.Should().Be(3);
            envelope.Payload.TagsScore.Should().BeEquivalentTo(new[]
            {
                new TagsScoreDto { Tag = "tag1", Score = 5 },
                new TagsScoreDto { Tag = "tag2", Score = 3 },
                new TagsScoreDto { Tag = "tag3", Score = 7 },
            });
        }

        [Fact]
        public async Task GivenNoTags_WhenGettingStats_ThenReturnsEmptyStats()
        {
            // Arrange
            IDictionary<string, long> stats = new Dictionary<string, long>();
            Factory.DraftRepositoryMock.Setup(m => m.GetTagsStats()).Returns(Task.FromResult(stats));

            // Act
            var response = await Client.GetAsync(TagStatsEndpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<TagsStatsResponseDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.MaxScore.Should().Be(0);
            envelope.Payload.AverageScore.Should().Be(0);
            envelope.Payload.Count.Should().Be(0);
            envelope.Payload.TagsScore.Should().BeNull();
        }

        [Fact]
        public async Task GivenException_WhenGettingStats_ThenReturnsInternalServerError()
        {
            // Arrange
            Factory.DraftRepositoryMock.Setup(m => m.GetTagsStats()).Throws<Exception>();

            // Act
            var response = await Client.GetAsync(TagStatsEndpoint);

            // Assert
            response.ShouldBeServerError();
        }

        [Fact]
        public async Task GivenNotFoundException_WhenGettingStats_ThenReturnsSuccessful()
        {
            // Arrange
            Factory.DraftRepositoryMock.Setup(m => m.GetTagsStats()).Throws(new NotFoundException(null));

            // Act
            var response = await Client.GetAsync(TagStatsEndpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<TagsStatsResponseDto>();
            envelope.ShouldBeUnsuccessful();
        }
    }
}
