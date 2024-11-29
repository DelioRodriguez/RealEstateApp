namespace RealEstateApp.Application.Dtos.ApiAccount
{
    public class RegisterResponseDto
    {
        public bool Success { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
