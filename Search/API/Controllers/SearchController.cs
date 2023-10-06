using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Search.Service;
using Shared;

namespace Search.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    static private readonly SearchSettings _searchSettings = new();
    private readonly SearchService _searchService = new();
    private readonly CommandService _commandService = new(_searchSettings);

    public SearchController() { }

    [HttpGet("{query}")]
    public SearchResult Search(string query)
    {
        return _searchService.Search(query.Split(","), _searchSettings);
    }

    [HttpGet("/")]
    public string ExecuteCommand(string command)
    {
        return _commandService.ProcessCommand(command);
    }
}