using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Mappers
{
    public static class CommunityMapper
    {
        public static Community MapToCommunity(this CreateCommunityDto communityDto)
        {
            return new Community
            {
                Name = communityDto.Name,
                Description = communityDto.Description,
                PfpUrl = communityDto.PfpUrl,
                MemberCount = 0
            };
        }

        public static CommunityDto MapToCommunityDto(this Community community)
        {
            return new CommunityDto
            {
                Name = community.Name,
                Description = community.Description,
                PfpUrl = community.PfpUrl
            };
        }
    }
}
