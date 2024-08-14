using System;

namespace CareerConnect.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public int JobSeekerId { get; set; } // Foreign key to JobSeeker
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Technologies { get; set; } // List of technologies used
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation property
        public JobSeeker? JobSeeker { get; set; }
    }
}
