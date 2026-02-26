using DevFreela.API.Database.Enums;

namespace DevFreela.API.Database.Entities
{
    public class User : BaseEntity
    {
        public ETypeUser Type { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public byte[]? ProfilePicture { get; set; }
        public List<Skill> Skills { get; set; } = [];
    }
}