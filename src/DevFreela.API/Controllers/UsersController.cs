using DevFreela.API.Models.Users;
using DevFreela.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET api/users
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _usersService.GetAllAsync();

            return Ok(result);
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Post(CreateUserInputModel model)
        {
            var id = await _usersService.CreateUserAsync(model);

            return CreatedAtAction(nameof(GetById), new { id }, model);
        }

        // GET api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT api/users/{id}/profile-picture
        [HttpPut("{id}/profile-picture")]
        public async Task<IActionResult> PostProfilePicture(Guid id, IFormFile file)
        {
            await _usersService.UpdateProfilePictureAsync(id, file);

            return NoContent();
        }
    }
}
