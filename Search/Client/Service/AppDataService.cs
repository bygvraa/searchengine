using System.Net.Http.Json;
using Shared;

namespace Client.Service
{
    public class AppDataService
    {
        private readonly HttpClient _http;
        private readonly string baseAddress = "";

        public AppDataService(HttpClient http)
        {
            _http = http;
            baseAddress = Config.LOADBALANCER_ADDRESS;
        }

        public async Task<SearchResult?> GetQuery(string query)
        {
            var url = $"{baseAddress}/Search/{query}";
            return await _http.GetFromJsonAsync<SearchResult>(url);
        }
    }
}