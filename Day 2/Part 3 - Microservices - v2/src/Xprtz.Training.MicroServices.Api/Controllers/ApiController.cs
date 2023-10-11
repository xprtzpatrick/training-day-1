using Microsoft.AspNetCore.Mvc;

namespace Xprtz.Training.MicroServices.Api.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    public static List<string> Messages { get; } = new();
    private readonly ILogger<ApiController> _logger;

    public ApiController(ILogger<ApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("GetLatestMessage")]
    public IActionResult Get()
    {
        if (!Messages.Any())
        {
            _logger.LogInformation("No messages on the bus yet...");
            return new OkObjectResult("No messages yet!");
        }

        return new OkObjectResult(string.Join("\n", Messages));
    }
}