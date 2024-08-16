namespace CareerConnect.DTO
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public int JobSeekerId {  get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Technologies { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Add any other necessary fields, but omit the JobSeeker navigation property
    }

}
