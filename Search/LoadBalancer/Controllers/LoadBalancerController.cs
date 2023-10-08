using Microsoft.AspNetCore.Mvc;

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

    private static int next = 0;
    private static readonly object mLock = new();

    public LoadBalancerController(ILogger<LoadBalancerController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("{query}")]
    public string Search(string query)
    {
        lock (mLock)
        {
            _logger.LogInformation($"LoadBalancer server requested - next = {next}");

            string server = $"{_servers[next]}/{query}";
            next = (next + 1) % _servers.Length;

            Response.Redirect(server);
        }
        return "";
    }
}
