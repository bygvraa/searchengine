using Microsoft.AspNetCore.Mvc;
using Database.Service;
using Shared.BE;

namespace Database.Controllers;

[ApiController]
[Route("[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly ILogger<DatabaseController> _logger;

    private readonly DatabaseService _databaseService;

    public DatabaseController(ILogger<DatabaseController> logger, DatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    [HttpGet("words")]
    public async Task<ActionResult<Dictionary<string, int>>> GetAllWords()
    {
        _logger.LogInformation("GET: GetAllWords requested at Database server");
        var result = await _databaseService.GetAllWords();

        if (result.Count() is not 0)
        {
            return result!;
        }
        else
        {
            _logger.LogError($"GET: GetAllWords requested at Database server --- ERROR");
            return null!;
        }
    }

    [HttpPost("documents")]
    public async Task<ActionResult<List<KeyValuePair<int, int>>>> GetDocuments([FromBody] List<int> wordIds)
    {
        _logger.LogInformation($"POST: GetDocuments requested at Database server: {wordIds.Count}");
        var result = await _databaseService.GetDocuments(wordIds);

        if (result.Count != 0)
        {
            return result!;
        }
        else
        {
            _logger.LogError($"POST: GetDocuments requested at Database server --- ERROR");
            return null!;
        }
    }

    [HttpPost("details")]
    public async Task<ActionResult<List<BEDocument>>> GetDocDetails([FromBody] List<int> docIds)
    {
        _logger.LogInformation($"POST: GetDocDetails requested at Database server: {docIds.Count}");
        var result = await _databaseService.GetDocDetails(docIds);

        if (result.Count != 0)
        {
            return result!;
        }
        else
        {
            _logger.LogError($"POST: GetDocDetails requested at Database server --- ERROR");
            return null!;
        }
    }
}
