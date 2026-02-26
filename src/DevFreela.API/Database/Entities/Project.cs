using DevFreela.API.Database.Enums;

namespace DevFreela.API.Database.Entities
{
    public class Project : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal TotalCost { get; set; } = 0;
        public EStageProject Stage { get; set; } = EStageProject.Created;
        public Guid ClientId { get; set; }
        public Guid FreelancerId { get; set; }
        public List<Skill> RequiredSkills { get; set; } = [];

        public virtual User Client { get; set; } = null!;
        public virtual User Freelancer { get; set; } = null!;
        public virtual List<Comment> Comments { get; set; } = [];
    }
}
