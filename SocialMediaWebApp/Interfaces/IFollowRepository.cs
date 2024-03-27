using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface IFollowRepository
    {
        Task<List<Following>> GetFollowingsOfMember(string memberId);
        Task<List<Following>> GetFollowingsOfCommunity(int communityId);
        Task<Following?> GetFollowingById(string memberId, int communityId);
        bool Follow(string memberId, int communityId);
        bool Unfollow(Following following);
        bool Save();
    }
}
