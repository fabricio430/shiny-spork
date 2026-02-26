using DevFreela.API.Models.Skills;
using DevFreela.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/skills")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillsService _skillsService;

        public SkillsController(ISkillsService skillsService)
        {
            _skillsService = skillsService;            
        }

        // GET api/skills
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var skills = await _skillsService.GetAllAsync();

            return Ok(skills);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var skill = await _skillsService.GetByIdAsync(id);
            if (skill == null)
            {
                return Problem(detail: "Skill not Found", statusCode: StatusCodes.Status404NotFound);
            }
            return Ok(skill);
        }

        // POST api/skills
        [HttpPost]
        public async Task<IActionResult> Post(CreateSkillInputModel model)
        {
            var id = await _skillsService.CreateAsync(model);

            return CreatedAtAction(nameof(GetById), new { id }, model);
        }
    }
}
