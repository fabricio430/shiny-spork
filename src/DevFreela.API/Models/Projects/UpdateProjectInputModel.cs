namespace DevFreela.API.Models.Projects
{
    public class UpdateProjectInputModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
    }
}
