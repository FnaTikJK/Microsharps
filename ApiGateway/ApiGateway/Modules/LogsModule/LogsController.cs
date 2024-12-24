using Infrastructure;
using Infrastructure.API;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Modules.LogsModule;

[Route("api/[controller]")]
[ApiController]
public class LogsController: ControllerBase
{
    private readonly ILogsService _logsService;
    
    public LogsController(ILogsService logsService)
    {
        _logsService = logsService;
    }
    /// <summary>
    /// Current day logs 
    /// </summary>
    /// <param name="date">Date in yyyy-MM-dd format</param>
    [HttpGet]
    public async Task<ActionResult<string>> GetTodayLogs() => await GetLogs(DateOnly.FromDateTime(DateTime.UtcNow).ToString());

    /// <summary>
    /// Logs for the specified date
    /// </summary>
    /// <param name="date">Date in yyyy-MM-dd format</param>
    [HttpGet(@"{date:regex([[\d*]])}")]
    public async Task<ActionResult<string>> GetLogs([FromRoute] string date)
    {
        if (!DateOnly.TryParse(date, out var dateOnly))
            return BadRequest("Incorrect date format. Should be yyyy.MM.dd");
        
        return (await _logsService.ReadLogAsync(dateOnly)).ActionResult();
    }
}