// Models/Summary.cs
namespace Portfolio.Api.Models
{
    public class Summary
    {
        public string FullName { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string LinkedInUrl { get; set; } = default!;
        public string GitHubUrl { get; set; } = default!;
        public string Bio { get; set; } = default!;
    }
}
