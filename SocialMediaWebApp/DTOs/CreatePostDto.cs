using SocialMediaWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaWebApp.DTOs
{
    public class CreatePostDto
    {
        public string Content { get; set; }
    }
}
