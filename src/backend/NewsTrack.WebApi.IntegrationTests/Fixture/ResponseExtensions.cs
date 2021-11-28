using FluentAssertions;
using NewsTrack.WebApi.Dtos;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

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
            return await JsonSerializer.DeserializeAsync<Envelope<T>>(content, SerializerOptions);
        }

        public static async Task<Envelope> ShouldBeVoid(this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Envelope>(content, SerializerOptions);
        }

        public static void ShouldBeSuccessful<T>(this Envelope<T> envelope)
            where T : class
        {
            envelope.IsSuccessful.Should().BeTrue();
            envelope.Payload.Should().NotBeNull();
            envelope.At.Should().BeBefore(System.DateTime.UtcNow);
            envelope.Error.Should().BeNull();
        }

        public static void ShouldBeUnsuccessful(this Envelope envelope)
        {
            envelope.IsSuccessful.Should().BeFalse();
            envelope.At.Should().BeBefore(System.DateTime.UtcNow);
            envelope.Error.Should().NotBeNull();
        }

        public static void ShouldBeSuccessful(this Envelope envelope)
        {
            envelope.IsSuccessful.Should().BeTrue();
            envelope.At.Should().BeBefore(System.DateTime.UtcNow);
            envelope.Error.Should().BeNull();
        }
    }
}
