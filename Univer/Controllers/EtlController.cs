using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EtlController : ControllerBase
{
    private readonly IEtlService _etlService;
    private readonly ILogger<EtlController> _logger;

    public EtlController(IEtlService etlService, ILogger<EtlController> logger)
    {
        _etlService = etlService;
        _logger = logger;
    }

    [HttpPost("run")]
    public async Task<IActionResult> RunEtl([FromQuery] int? maxThreads)
    {
        try
        {
            _logger.LogInformation("Starting ETL process");
            await _etlService.RunEtlProcess(maxThreads ?? 5);
            return Ok("ETL process completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ETL process failed");
            return StatusCode(500, "ETL process failed");
        }
    }
}