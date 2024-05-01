using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Application.Dtos
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
