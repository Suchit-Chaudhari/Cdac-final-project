using CareerConnect.Models;

public class JobSeeker
{
    public int JobSeekerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public string Skills { get; set; }
    public string? Resume { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public User? User { get; set; }
    public ICollection<Application>? Applications { get; set; } // Initialize or set to null as needed
    public ICollection<Project>? Projects { get; set; } // New navigation property
}
