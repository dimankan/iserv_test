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
    public async Task<IActionResult> Get(
        [FromQuery] string country = null,
        [FromQuery] string name = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var (items, totalCount) = await _service.GetUniversities(
            country, name, page, pageSize);

        return Ok(new
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = items
        });
    }
}