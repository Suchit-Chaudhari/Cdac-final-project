using CareerConnect.Models;

public class Employer
{
    public int EmployerId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyDescription { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public User User { get; set; }
    public ICollection<Job> Jobs { get; set; } = new List<Job>(); // Initialize to avoid null issues
}
