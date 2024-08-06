namespace CareerConnect.DTO
{
    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public JobSeekerDto? JobSeeker { get; set; }
        public EmployerDto? Employer { get; set; }

        public string? ProfilePicture { get; set; }  // Add ProfilePicture here


    }
}
