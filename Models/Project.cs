// Models/Project.cs
namespace Portfolio.Api.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<string> TechStack { get; set; } = new();
        public string? Link { get; set; }
    }
}
