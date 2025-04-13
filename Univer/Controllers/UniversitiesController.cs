using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UniversitiesController : ControllerBase
{
    private readonly IUniversitiesService _service;

    public UniversitiesController(IUniversitiesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string country = null,[FromQuery] string name = null)
    {
        var universities = await _service.GetUniversities(country, name);
        if(universities == null)
            return NotFound(); 
        
        return Ok(universities);
    }
}