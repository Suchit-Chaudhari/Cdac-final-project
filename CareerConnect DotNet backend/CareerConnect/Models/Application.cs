namespace CareerConnect.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public int JobSeekerId { get; set; }
        public string Status { get; set; }
        public string CoverLetter { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Job? Job { get; set; }
        public JobSeeker? JobSeeker { get; set; }
    }
}
