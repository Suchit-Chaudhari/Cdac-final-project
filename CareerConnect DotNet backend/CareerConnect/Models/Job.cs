namespace CareerConnect.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public int EmployerId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Location { get; set; }
        public decimal Salary { get; set; }
        public string JobType { get; set; }
        public string ExperienceLevel { get; set; }
        public string Industry { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Employer? Employer { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }
}
