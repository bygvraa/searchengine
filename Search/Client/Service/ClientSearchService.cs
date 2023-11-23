using System.Net.Http.Json;
using Shared;

namespace Client.Service;

public class ClientSearchService
{
    private readonly ILogger<ClientSearchService> _logger;
    private readonly HttpClient _http;

    public ClientSearchService(ILogger<ClientSearchService> logger, HttpClient http)
    {
        _logger = logger;
        _http = http;
    }

    public async Task<SearchResult?> GetSearchResult(Search search)
    {
        var route = $"{_http.BaseAddress}search";
        var response = await _http.PostAsJsonAsync(route, search);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<SearchResult>();
            return result;
        }
        return null;
    }
}
