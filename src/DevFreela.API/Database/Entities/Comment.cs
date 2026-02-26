namespace DevFreela.API.Database.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; } = null!;
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public virtual Project Project { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}