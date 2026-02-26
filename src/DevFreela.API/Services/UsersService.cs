using DevFreela.API.Database;
using DevFreela.API.Database.Entities;
using DevFreela.API.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Services
{
    public interface IUsersService
    {
        Task<Guid> CreateUserAsync(CreateUserInputModel model);
        Task<List<UserViewModel>> GetAllAsync();
        Task<UserViewModel?> GetByIdAsync(Guid id);
        Task UpdateProfilePictureAsync(Guid id, IFormFile formFile);
    }

    public class UsersService(ApplicationDbContext context) : IUsersService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<UserViewModel>> GetAllAsync()
        {
            var users = await _context.Users
                .Where(u => u.DeletedAt == null)
                .ToListAsync();

            return users.Select(u => (UserViewModel)u!).ToList() ;
        }

        public async Task<UserViewModel?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.Include(x => x.Skills).FirstOrDefaultAsync(x => x.Id == id);

            return (UserViewModel?)user;
        }

        public async Task<Guid> CreateUserAsync(CreateUserInputModel model)
        {
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Type = model.Type
            };

            user.Skills.AddRange(_context.Skills.Where(x => model.SkillsIds.Contains(x.Id)));

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateProfilePictureAsync(Guid id, IFormFile formFile)
        {
            var user = await _context.Users.Include(x => x.Skills).FirstOrDefaultAsync(x => x.Id == id);

            var bytes = new byte[formFile.Length];
            await formFile.CopyToAsync(new MemoryStream(bytes));

            user.ProfilePicture = bytes;

            await _context.SaveChangesAsync();
        }
    }
}
