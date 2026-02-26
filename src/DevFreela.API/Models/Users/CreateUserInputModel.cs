using DevFreela.API.Database.Enums;

namespace DevFreela.API.Models.Users
{
    public class CreateUserInputModel
    {
        public ETypeUser Type { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public byte[]? ProfilePicture { get; set; } = null!;
        public List<Guid> SkillsIds { get; set; } = [];
    }
}