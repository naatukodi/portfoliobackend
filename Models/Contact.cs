// Models/Contact.cs
namespace Portfolio.Api.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string LinkedInUrl { get; set; } = default!;
        public string GitHubUrl { get; set; } = default!;
    }
}
