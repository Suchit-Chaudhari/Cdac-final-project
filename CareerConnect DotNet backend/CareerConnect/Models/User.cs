using CareerConnect.Models;

public class User
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string? ProfilePicture { get; set; }  // Moved ProfilePicture here
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public JobSeeker? JobSeeker { get; set; }
    public Employer? Employer { get; set; }
}
