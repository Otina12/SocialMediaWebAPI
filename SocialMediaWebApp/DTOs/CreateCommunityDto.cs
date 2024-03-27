using SocialMediaWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaWebApp.DTOs
{
    public class CreateCommunityDto
    {
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public string? PfpUrl { get; set; }
    }
}
