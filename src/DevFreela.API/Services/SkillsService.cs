using DevFreela.API.Database;
using DevFreela.API.Database.Entities;
using DevFreela.API.Models.Skills;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Services
{
    public interface ISkillsService
    {
        Task<List<SkillViewModel>> GetAllAsync();
        Task<SkillViewModel?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CreateSkillInputModel inputModel);
    }

    public class SkillsService(ApplicationDbContext dbContext) : ISkillsService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<SkillViewModel>> GetAllAsync()
        {
            var skills = await _dbContext.Skills.ToListAsync();

            return skills.Select(s => (SkillViewModel)s!).ToList();
        }

        public async Task<SkillViewModel?> GetByIdAsync(Guid id)
        {
            var skill = await _dbContext.Skills.FirstOrDefaultAsync(s => s.Id == id);

            return (SkillViewModel?)skill;
        }

        public async Task<Guid> CreateAsync(CreateSkillInputModel inputModel)
        {
            var skill = new Skill()
            {
                Name = inputModel.Name
            };

            _dbContext.Skills.Add(skill);

            await _dbContext.SaveChangesAsync();

            return skill.Id;
        }
    }
}
