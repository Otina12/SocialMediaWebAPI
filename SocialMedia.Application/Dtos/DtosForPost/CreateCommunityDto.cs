using SocialMedia.Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Application.Dtos.DtosForPost
{
    public class CreateCommunityDto
    {
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public string? PfpUrl { get; set; }
    }
}
