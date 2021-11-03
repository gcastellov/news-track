﻿using System.Net.Http;
using Xunit;
using System;
using System.Web;
using NewsTrack.WebApi.Dtos;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text.Json;

namespace NewsTrack.WebApi.IntegrationTests.Fixture
{
    public class BaseTest : IClassFixture<TestWebAppFactory<Startup>>
    {
        private const string AuthenticationEndpoint = "/api/authentication/generate";

        protected TestWebAppFactory<Startup> Factory { get; }
        protected HttpClient Client { get; }

        protected string Token { get; private set; }

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
            Token = idResponse.GetProperty("token").GetString();
        }

        protected Uri GetUriWithQueryString(string path, params Tuple<string, object>[] parameters)
        {
            var uriBuilder = new UriBuilder(Client.BaseAddress);
            uriBuilder.Path = path;
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            
            foreach(var param in parameters)
            {
                query[param.Item1] = param.Item2.ToString();
            }

            uriBuilder.Query = query.ToString();
            var uri = uriBuilder.ToString();
            return new Uri(uri);
        }
    }
}
