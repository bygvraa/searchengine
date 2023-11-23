using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Server.Service;
using Shared;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;

    private readonly SearchSettings _searchSettings;
    private readonly SearchService _searchService;
    private readonly CommandService _commandService;

    public SearchController(ILogger<SearchController> logger, SearchSettings searchSettings, SearchService searchService)
    {
        _logger = logger;
        _searchSettings = searchSettings;
        _searchService = searchService;
        _commandService = new CommandService(_searchSettings);
    }

    [HttpGet("{query}")]
    public async Task<ActionResult<SearchResult>> GetQuery(string query)
    {
        _logger.LogInformation($"GET: GetQuery request at server: {query}");
        try
        {
            var result = await _searchService.Search(query.Split(","), _searchSettings);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing search");
            return BadRequest("Error processing search");
        }
    }

    [HttpPost]
    public async Task<ActionResult<SearchResult>> GetSearch([FromBody] Search search)
    {
        _logger.LogInformation($"POST: GetSearch request at server: {search.Query}");
        try
        {
            var result = await _searchService.Search(search.Query.Split(","), search.Settings);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing search");
            return BadRequest("Error processing search");
        }
    }

    [HttpGet("/{command}")]
    public ActionResult<string> ExecuteCommand(string command)
    {
        var result = _commandService.ProcessCommand(command);
        return Ok(result);
    }
}