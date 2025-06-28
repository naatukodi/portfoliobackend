// Models/Education.cs
namespace Portfolio.Api.Models
{
    public class Education
    {
        public int Id { get; set; }
        public string Institution { get; set; } = default!;
        public string Degree { get; set; } = default!;
        public string FieldOfStudy { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? GPA { get; set; }
        public string Location { get; set; } = default!;
    }
}
