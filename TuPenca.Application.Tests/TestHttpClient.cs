using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace TuPenca.Application.Tests
{
    public class TestHttpClient
    {
        private readonly HttpClient _client;

        public TestHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<JsonElement> Post(string url, object body)
        {
            var resp = await _client.PostAsJsonAsync(url, body);
            var content = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"ERROR {resp.StatusCode}: {content}");

            return JsonSerializer.Deserialize<JsonElement>(content);
        }

        public async Task<JsonElement> Get(string url)
        {
            var resp = await _client.GetAsync(url);
            var content = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"ERROR {resp.StatusCode}: {content}");

            return JsonSerializer.Deserialize<JsonElement>(content);
        }

        public async Task Put(string url, object body)
        {
            var resp = await _client.PutAsJsonAsync(url, body);
            var content = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"ERROR {resp.StatusCode}: {content}");
        }

        public void SetBearer(string token)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public void SetHeader(string key, string value)
        {
            _client.DefaultRequestHeaders.Remove(key);
            _client.DefaultRequestHeaders.Add(key, value);
        }

        public void ClearHeaders()
        {
            _client.DefaultRequestHeaders.Clear();
        }
    }
}
