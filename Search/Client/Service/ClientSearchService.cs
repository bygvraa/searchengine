using System.Net.Http.Json;
using Shared;

namespace Client.Service;

public class ClientSearchService
{
    private readonly ILogger<ClientSearchService> _logger;
    private readonly HttpClient _httpClient;

    public ClientSearchService(ILogger<ClientSearchService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<SearchResult?> GetSearchResult(Search search)
    {
        var route = $"{_httpClient.BaseAddress}search";
        var response = await _httpClient.PostAsJsonAsync(route, search);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SearchResult>();
        }
        return null;
    }
}
