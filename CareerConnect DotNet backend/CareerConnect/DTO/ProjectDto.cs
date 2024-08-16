using Newtonsoft.Json;

namespace CareerConnect.DTO
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public int JobSeekerId {  get; set; }
        public string ProjectName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("technologies")]
        public string Technologies { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Add any other necessary fields, but omit the JobSeeker navigation property
    }

}
