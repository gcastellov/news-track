using System.Net.Http;
using Xunit;
using System;
using System.Web;
using NewsTrack.WebApi.Dtos;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using NewsTrack.Domain.Entities;
using FluentAssertions;

namespace NewsTrack.WebApi.IntegrationTests.Fixture
{
    public class BaseTest : IClassFixture<TestWebAppFactory<Startup>>
    {
        protected const string AuthenticationEndpoint = "/api/authentication/generate";

        protected TestWebAppFactory<Startup> Factory { get; }
        protected HttpClient Client { get; }        

        protected BaseTest(TestWebAppFactory<Startup> testWebAppFactory)
        {
            Factory = testWebAppFactory;
            Client = Factory.CreateClient();
        }

        protected async Task Authenticate()
        {
            var payload = new AuthenticationDto
            {
                Username = Factory.Identity.Email,
                Password = Factory.IdentityClearPassword,
            };

            Factory.IdentityRepositoryMock.Setup(m => m.GetByEmail(payload.Username))
                .Returns(Task.FromResult(Factory.Identity));

            var response = await Client.PostAsJsonAsync(AuthenticationEndpoint, payload);
            response.ShouldBeSuccessful();

            string content = await response.Content.ReadAsStringAsync();
            var envelope = JsonDocument.Parse(content);
            var idResponse = envelope.RootElement.GetProperty("payload");
            Factory.Token = idResponse.GetProperty("token").GetString();
        }

        protected async Task<HttpResponseMessage> AuthenticatedPatch(string endpoint)
        {
            if (Factory.Token == null)
            {
                await Authenticate();
            }

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Factory.Token}");
            return await Client.PatchAsync(endpoint, null);
        }

        protected async Task<HttpResponseMessage> AuthenticatedPost<T>(string endpoint, T payload)
        {
            if (Factory.Token == null)
            {
                await Authenticate();
            }

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Factory.Token}");
            return await Client.PostAsJsonAsync(endpoint, payload);
        }

        protected async Task<HttpResponseMessage> AuthenticatedPost(string endpoint)
        {
            if (Factory.Token == null)
            {
                await Authenticate();
            }

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Factory.Token}");
            return await Client.PostAsync(endpoint, new StringContent(""));
        }

        protected async Task<HttpResponseMessage> AuthenticatedGet(string endpoint)
        {
            if (Factory.Token == null)
            {
                await Authenticate();
            }

            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Factory.Token}");
            return await Client.GetAsync(endpoint);
        }

        protected Uri GetUriWithQueryString(string path, params (string, object)[] parameters)
        {
            var uriBuilder = new UriBuilder(Client.BaseAddress);
            uriBuilder.Path = path;
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            
            foreach(var param in parameters)
            {
                query[param.Item1] = param.Item2?.ToString();
            }

            uriBuilder.Query = query.ToString();
            var uri = uriBuilder.ToString();
            return new Uri(uri);
        }

        protected static void AssertNewsDto(Draft draftResult, NewsDto dto)
        {
            dto.Id.Should().Be(draftResult.Id);
            dto.Title.Should().Be(draftResult.Title);
            dto.CreatedAt.Should().Be(draftResult.CreatedAt);
            dto.Picture.Should().Be(draftResult.Picture);
            dto.Related.Should().Be(draftResult.Related);
            dto.Tags.Should().BeEquivalentTo(draftResult.Tags);
            dto.Paragraphs.Should().BeEquivalentTo(draftResult.Paragraphs);
            dto.CreatedBy.Should().Be(draftResult.User.Username);
            dto.Fucks.Should().Be(draftResult.Fucks);
            dto.Views.Should().Be(draftResult.Views);
            dto.Uri.Should().Be(draftResult.Uri);
        }

        protected static void AssertDigestDto(Draft draftResult, NewsDigestDto dto)
        {
            dto.Id.Should().Be(draftResult.Id);
            dto.Title.Should().Be(draftResult.Title);
            dto.Fucks.Should().Be(draftResult.Fucks);
            dto.Views.Should().Be(draftResult.Views);
            dto.Url.Should().Be(draftResult.Uri);
        }

        protected static void AssertCommentDto(Comment comment, CommentDto dto)
        {
            dto.Id.Should().Be(comment.Id);
            dto.Content.Should().Be(comment.Content);
            dto.DraftId.Should().Be(comment.DraftId);
            dto.ReplyingTo.Should().Be(comment.ReplyingTo);
            dto.CreatedAt.Should().Be(comment.CreatedAt);
            dto.CreatedBy.Should().Be(comment.CreatedBy.Username);
            dto.Likes.Should().Be(comment.Likes);
            dto.Replies.Should().Be(comment.Replies);
        }

        protected static Draft CreateDraftEntity()
            => new Draft
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Fucks = 3,
                Related = 2,
                Uri = new Uri("http://www.some.com/path/resource"),
                Picture = new Uri("http://www.some.com/path/img.png"),
                Title = "The title",
                Views = 554,
                Paragraphs = new[]
                {
                    "First paragraph",
                    "Second paragraph",
                    "Third paragraph"
                },
                Tags = new[]
                {
                    "tag1",
                    "tag2"
                },
                User = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "someuser",
                },
                Website = "www.some.com",
            };

        protected static Comment CreateCommentEntity()
            => new Comment
            {
                Content = "Some content",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "someuser",
                },
                DraftId = Guid.NewGuid(),
                Likes = 45,
                Replies = 2,
                ReplyingTo = Guid.NewGuid(),
            };
    }
}
