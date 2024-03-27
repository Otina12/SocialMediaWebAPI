using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.DTOs
{
    public class MemberEditDto
    {
        public string? UserName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePhotoUrl { get; set; }
    }
}
