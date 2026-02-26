using DevFreela.API.Database.Entities;

namespace DevFreela.API.Models.Skills
{
    public class SkillViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;


        public static implicit operator SkillViewModel?(Skill? skill)
        {
            if (skill == null) return null;

            return new SkillViewModel()
            {
                Id = skill.Id,
                Name = skill.Name
            };
        }
    }
}