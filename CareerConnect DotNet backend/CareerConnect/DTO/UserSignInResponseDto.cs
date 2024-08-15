namespace CareerConnect.DTO
{
    public class UserSignInResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Role { get; set; }     
        public string Token { get; set; }
    }
}
