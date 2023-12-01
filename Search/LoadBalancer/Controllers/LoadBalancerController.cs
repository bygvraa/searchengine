using Microsoft.AspNetCore.Mvc;
using Shared;

namespace LoadBalancer.Controllers;

[ApiController]
[Route("search")]
public class LoadBalancerController : ControllerBase
{
    private readonly ILogger<LoadBalancerController> _logger;

    private static readonly string[] _servers = {
        "https://localhost:7233/search",
        "https://localhost:7234/search"
    };

    private static int _next = 0;
    private static readonly object _lock = new();

    public LoadBalancerController(ILogger<LoadBalancerController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{query}")]
    public void RedirectGet(string query)
    {
        lock (_lock)
        {
            _logger.LogInformation($"LoadBalancer server requested - next = {_next}");

            string server = $"{_servers[_next]}/{query}";
            _next = (_next + 1) % _servers.Length;

            Response.Redirect(server);
        }
    }

    [HttpPost]
    public async Task<ActionResult<object?>> RedirectPost([FromBody] Search search)
    {
        _logger.LogInformation($"LoadBalancer server requested - next = {_next}");

        string server = _servers[_next];
        _next = (_next + 1) % _servers.Length;

        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync(server, search);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<object>();
            return Ok(result);
        }
        else
        {
            return BadRequest("Error in API server");
        }
    }
}
