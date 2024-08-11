namespace CareerConnect.DTO
{
    public class JobCreateDto
    {
        public int EmployerId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Location { get; set; }
        public decimal Salary { get; set; }
        public string JobType { get; set; }
        public string ExperienceLevel { get; set; }
        public string Industry { get; set; }
    }
}
