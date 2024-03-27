using System.ComponentModel.DataAnnotations;

namespace SocialMediaWebApp.DTOs
{
    public class MemberRegisterDto
    {
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
