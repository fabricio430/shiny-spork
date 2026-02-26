namespace DevFreela.API.Models.Projects
{
    public class CreateProjectInputModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal TotalCost { get; set; }
        public Guid ClientId { get; set; }
        public Guid FreelancerId { get; set; }
        public List<Guid> RequiredSkilsIds { get; set; } = [];
    }
}
