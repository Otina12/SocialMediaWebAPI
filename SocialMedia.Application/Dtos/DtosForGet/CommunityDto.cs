namespace SocialMedia.Application.Dtos.DtosForGet
{
    public class CommunityDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? PfpUrl { get; set; }
    }
}
