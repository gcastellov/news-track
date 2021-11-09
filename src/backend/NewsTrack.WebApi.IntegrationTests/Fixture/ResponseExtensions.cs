using FluentAssertions;
using NewsTrack.WebApi.Dtos;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace NewsTrack.WebApi.IntegrationTests.Fixture
{
    internal static class ResponseExtensions
    {
        internal static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static void ShouldBeSuccessful(this HttpResponseMessage response)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        public static void ShouldBeServerError(this HttpResponseMessage response)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }

        public static void ShouldBeBadRequest(this HttpResponseMessage response)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        public static void ShouldBeAccepted(this HttpResponseMessage response)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Accepted);
        }

        public static void ShouldBeRedirectedTo(this HttpResponseMessage response, string url)
        {
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            response.RequestMessage.RequestUri.Should().Be(url);
        }

        public static async Task<Envelope<T>> ShouldBeOfType<T>(this HttpResponseMessage response)
            where T: class
        {
            var content = await response.Content.ReadAsStreamAsync();           
            var payload = await JsonSerializer.DeserializeAsync<Envelope<T>>(content, SerializerOptions);
            return payload;
        }

        public static void ShouldBeSuccessful<T>(this Envelope<T> envelope)
            where T : class
        {
            envelope.IsSuccessful.Should().BeTrue();
            envelope.At.Should().BeBefore(System.DateTime.UtcNow);
            envelope.ErrorMessage.Should().BeNull();
        }

        public static void ShouldBeUnsuccessful<T>(this Envelope<T> envelope)
            where T : class
        {
            envelope.IsSuccessful.Should().BeFalse();
            envelope.At.Should().BeBefore(System.DateTime.UtcNow);
            envelope.ErrorMessage.Should().NotBeNullOrEmpty();
            envelope.Payload.Should().BeNull();
        }
    }
}
