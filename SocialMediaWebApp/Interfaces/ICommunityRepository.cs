using SocialMediaWebApp.Helpers;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface ICommunityRepository
    {
        Task<List<Post>> GetAllPostsOfCommunity(int communityId, QueryObject query);
        Task<List<Post>> GetAllPostsOfCommunity(int communityId);
        Task<List<Member>> GetAllFollowersOfCommunity(int communityId);
        Task<bool> CommunityExists(int communityId);
        Task<Community?> GetCommunityById(int communityId);
        int GetFirstAvailableId();
        bool Create(Community community);
        bool Update(Community community);
        bool Delete(Community community);
        bool Save();
    }
}
