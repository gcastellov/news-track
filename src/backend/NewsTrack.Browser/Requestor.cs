using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewsTrack.Browser
{
    public class Requestor : IRequestor
    {
        private readonly HttpClient _httpClient;

        public Requestor()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> Get(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}
