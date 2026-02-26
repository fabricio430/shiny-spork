using DevFreela.API.Database.Entities;

namespace DevFreela.API.Models.Projects
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal TotalCost { get; set; } = 0;
        public List<Skill> RequiredSkills { get; set; } = [];
        public virtual User Client { get; set; } = null!;
        public virtual User Freelancer { get; set; } = null!;
        public virtual List<Comment> Comments { get; set; } = [];


        public static implicit operator ProjectViewModel?(Project? project)
        {
            if (project == null) return null;

            return new ProjectViewModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                TotalCost = project.TotalCost,
                RequiredSkills = project.RequiredSkills,
                Client = project.Client,
                Freelancer = project.Freelancer,
                Comments = project.Comments
            };
        }
    }
}