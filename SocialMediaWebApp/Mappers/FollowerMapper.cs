using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Mappers
{
    public static class FollowerMapper
    {
        public static FollowerDto MapToFollowerDto(this Member member)
        {
            return new FollowerDto
            {
                UserName = member.UserName!,
                Bio = member.Bio,
                ProfilePhotoUrl = member.ProfilePhotoUrl,
                JoinDate = member.JoinDate
            };
        }
    }
}
