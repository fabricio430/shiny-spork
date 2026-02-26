namespace DevFreela.API.Database.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public void TriggerUpdate()
        {
            UpdatedAt = DateTime.Now;
        }

        public void TriggerSoftDelete()
        {
            DeletedAt = DateTime.Now;
        }
    }
}
