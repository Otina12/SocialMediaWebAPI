using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Application.Dtos
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
