using System.ComponentModel.DataAnnotations;

namespace SocialMediaWebApp.DTOs
{
    public class MemberLoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
