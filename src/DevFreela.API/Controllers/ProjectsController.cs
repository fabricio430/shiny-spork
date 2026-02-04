using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    // api/projects/search=term
    [HttpGet]
    public IActionResult Get(string search = "")
    {
        return Ok();
    }
}
