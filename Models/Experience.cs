// Models/Experience.cs
namespace Portfolio.Api.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string Company { get; set; } = default!;
        public string Role { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Location { get; set; } = default!;
        public List<string> Responsibilities { get; set; } = new();
    }
}
