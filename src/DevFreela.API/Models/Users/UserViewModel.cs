using DevFreela.API.Database.Entities;
using DevFreela.API.Database.Enums;

namespace DevFreela.API.Models.Users
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public ETypeUser TypeUser { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[]? ProfilePicture { get; set; } = null!;
        public List<Skill> Skills { get; set; } = [];

        public static implicit operator UserViewModel?(User? user)
        {
            if (user == null) return null;

            return new UserViewModel
            {
                Id = user.Id,
                TypeUser = user.Type,
                Name = user.Name,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                Skills = user.Skills
            };
        }
    }
}