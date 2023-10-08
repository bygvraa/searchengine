using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Search.Service;
using Shared;

namespace Search.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly SearchSettings _searchSettings;
    private readonly SearchService _searchService;
    private readonly CommandService _commandService;

    public SearchController(SearchSettings searchSettings, SearchService searchService)
    {
        _searchSettings = searchSettings;
        _searchService = searchService;
        _commandService = new CommandService(_searchSettings);
    }

    [HttpGet("{query}")]
    public SearchResult Search(string query)
    {
        return _searchService.Search(query.Split(","), _searchSettings);
    }

    [HttpGet("/{command}")]
    public string ExecuteCommand(string command)
    {
        return _commandService.ProcessCommand(command);
    }
}