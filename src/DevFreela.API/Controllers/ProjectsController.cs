using DevFreela.API.Models.Config;
using DevFreela.API.Models.Projects;
using DevFreela.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController(
        IOptions<FreelanceTotalCostConfig> options,
        IProjectService projectService
            ) : ControllerBase
    {
        private readonly FreelanceTotalCostConfig _config = options.Value;
        private readonly IProjectService _projectService = projectService;

        // GET api/projects?search=crm
        [HttpGet]
        public async Task<IActionResult> Get(string search = "")
        {
            var result = await _projectService.Search(search);

            return Ok(result);
        }

        // GET api/projects/1234
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _projectService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST api/projects
        [HttpPost]
        public async Task<IActionResult> Post(CreateProjectInputModel model)
        {
            if (model.TotalCost < _config.Minimum || model.TotalCost > _config.Maximum)
            {
                return Problem("Numero fora dos limites.");
            }

            var id = await _projectService.CreateAsync(model);

            return CreatedAtAction(nameof(GetById), new { id }, model);
        }

        // PUT api/projects/1234
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateProjectInputModel model)
        {
            if (model.TotalCost < _config.Minimum || model.TotalCost > _config.Maximum)
            {
                return Problem(detail: "Numero fora dos limites.", statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            if (model.Id != id)
            {
                return Problem(detail: "O id do projeto deve ser igual ao id da URL.", statusCode: StatusCodes.Status422UnprocessableEntity);
            }

            await _projectService.UpdateAsync(id, model);

            return NoContent();
        }

        //  POST api/projects/1234/disable
        [HttpPost("{id}/disable")]
        public async Task<ActionResult> Disable(Guid id)
        {
            await _projectService.SoftDeleteAsync(id);

            return NoContent();
        }

        //  DELETE api/projects/1234
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _projectService.DeleteAsync(id);

            return NoContent();
        }

        // PUT api/projects/1234/start
        [HttpPut("{id}/start")]
        public async Task<IActionResult> Start(Guid id)
        {
            await _projectService.SetStartedAsync(id);

            return NoContent();
        }

        // PUT api/projects/1234/complete
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            await _projectService.SetCompletedAsync(id);

            return NoContent();
        }

        // POST api/projects/1234/comments
        [HttpPost("{id}/comments")]
        public async Task<IActionResult> PostComment(Guid id, CreateProjectCommentInputModel model)
        {
            await _projectService.AddCommentAsync(id, model);

            return Ok();
        }
    }
}
