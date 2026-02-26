using DevFreela.API.Database;
using DevFreela.API.Database.Entities;
using DevFreela.API.Database.Enums;
using DevFreela.API.Models.Projects;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Services
{
    public interface IProjectService
    {
        Task<List<ProjectViewModel>> Search(string search);
        Task<ProjectViewModel?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CreateProjectInputModel inputModel);
        Task UpdateAsync(Guid id, UpdateProjectInputModel inputModel);
        Task SoftDeleteAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task SetStartedAsync(Guid id);
        Task SetCompletedAsync(Guid id);
        Task AddCommentAsync(Guid id, CreateProjectCommentInputModel model);
    }

    public class ProjectService(ApplicationDbContext dbContext) : IProjectService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<ProjectViewModel>> Search(string search)
        {
            var projects = await _dbContext.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Include(p => p.RequiredSkills)
                .Where(p => (string.IsNullOrEmpty(search) || p.Title.Contains(search) || p.Description.Contains(search)) 
                        && p.DeletedAt == null)
                .ToListAsync();

            return projects.Select(p => (ProjectViewModel)p!).ToList();
        }

        public async Task<ProjectViewModel?> GetByIdAsync(Guid id)
        {
            var project = await _dbContext.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Include(p => p.RequiredSkills)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            return (ProjectViewModel?)project;
        }

        public async Task<Guid> CreateAsync(CreateProjectInputModel inputModel)
        {
            var skills = _dbContext.Skills.Where(s => inputModel.RequiredSkilsIds.Contains(s.Id)).ToList();

            var project = new Project()
            {
                Title = inputModel.Title,
                Description = inputModel.Description,
                TotalCost = inputModel.TotalCost,
                ClientId = inputModel.ClientId,
                FreelancerId = inputModel.FreelancerId,
                RequiredSkills = skills
            };

            _dbContext.Projects.Add(project);

            await _dbContext.SaveChangesAsync();

            return project.Id;
        }

        public async Task UpdateAsync(Guid id, UpdateProjectInputModel inputModel)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new InvalidOperationException("Projeto não encontrado.");

            project.Title = inputModel.Title;
            project.Description = inputModel.Description;
            project.TotalCost = inputModel.TotalCost;
            project.TriggerUpdate();

            await _dbContext.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new InvalidOperationException("Projeto não encontrado.");

            project.TriggerSoftDelete();

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new InvalidOperationException("Projeto não encontrado.");

            _dbContext.Projects.Remove(project);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SetStartedAsync(Guid id)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new InvalidOperationException("Projeto não encontrado.");

            project.Stage = EStageProject.Started;

            await _dbContext.SaveChangesAsync();
        }

        public async Task SetCompletedAsync(Guid id)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new InvalidOperationException("Projeto não encontrado.");

            project.Stage = EStageProject.Completed;

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddCommentAsync(Guid id, CreateProjectCommentInputModel model)
        {
            var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new InvalidOperationException("Projeto não encontrado.");

            var comment = new Comment()
            {
                ProjectId = id,
                Content = model.Content,
                UserId = model.UserId
            };

            _dbContext.Add(comment);

            await _dbContext.SaveChangesAsync();
        }
    }
}
