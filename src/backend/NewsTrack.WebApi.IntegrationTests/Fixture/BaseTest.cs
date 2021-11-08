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
        private const string AuthenticationEndpoint = "/api/authentication/generate";

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
                Username = "some@mailaddress.com",
                Password = "somepassword"
            };

            Factory.IdentityRepositoryMock.Setup(m => m.GetByEmail(payload.Username))
                .Returns(Task.FromResult(new Identity.Identity
                {
                    Email = payload.Username,
                    CreatedAt = DateTime.UtcNow,
                    IsEnabled = true,
                    Username = "someusername",
                    Password = "$2a$11$OR.oPIrkeMJrZ8/inLuSmO6SFCn5ZM.aLQCkHq3Sm/s.FfDVkGoKu",
                    IdType = Identity.IdentityTypes.Admin
                }));

            var response = await Client.PostAsJsonAsync(AuthenticationEndpoint, payload);
            response.ShouldBeSuccessful();

            string content = await response.Content.ReadAsStringAsync();
            var envelope = JsonDocument.Parse(content);
            var idResponse = envelope.RootElement.GetProperty("payload");
            Factory.Token = idResponse.GetProperty("token").GetString();
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

        protected Uri GetUriWithQueryString(string path, params Tuple<string, object>[] parameters)
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

        protected static void AssertDto(Draft draftResult, NewsDto dto)
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
    }
}
