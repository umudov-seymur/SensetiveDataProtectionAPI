using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LogController : ControllerBase
{
    private readonly ILoggingService _loggingService;

    public LogController(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    [HttpGet]
    public IActionResult GetLogs()
    {
        var logs = _loggingService.GetAllLogs();
        return Ok(logs);
    }
}