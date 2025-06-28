// Models/Skill.cs
namespace Portfolio.Api.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;  // e.g. “Cloud”, “DevOps”, “Language”
        public int Proficiency { get; set; }             // e.g. 1–5 scale
    }
}
