using Microsoft.AspNetCore.Mvc;
using Search.Services;
using Shared;

namespace Search.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly SearchSettings _searchSettings;
    private readonly SearchService _searchService;
    private readonly CommandService _commandService;

    public SearchController(SearchService searchService, SearchSettings searchSettings, CommandService commandService)
    {
        _searchService = searchService;
        _searchSettings = searchSettings;
        _commandService = commandService;
    }

    [HttpGet("{query}")]
    public IActionResult Search(string query)
    {
        var keywords = query.Split(",", StringSplitOptions.RemoveEmptyEntries);
        var result = _searchService.Search(keywords, _searchSettings);
        return Ok(result);
    }

    [HttpGet("/")]
    public IActionResult ExecuteCommand(string command)
    {
        var result = _commandService.ProcessCommand(command);
        return Ok(result);
    }
}