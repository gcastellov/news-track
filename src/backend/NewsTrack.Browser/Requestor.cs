using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewsTrack.Browser
{
    internal class Requestor : IRequestor
    {
        private readonly Lazy<HttpClient> _httpClient;

        public Requestor()
        {
            _httpClient = new Lazy<HttpClient>();
        }

        public async Task<string> Get(Uri uri)
        {
            var response = await _httpClient.Value.GetAsync(uri);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var payload = await response.Content.ReadAsByteArrayAsync();
                return Encoding.UTF8.GetString(payload, 0, payload.Length - 1);
            }

            return null;
        }

        public void Dispose()
        {
            if (_httpClient.IsValueCreated)
            {
                _httpClient.Value.Dispose();
            }
        }
    }
}
