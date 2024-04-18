using SocialMediaWebApp.Core.IRepositories;
using SocialMediaWebApp.Helpers;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface ICommunityRepository
    {
        Task<List<Post>> GetAllPostsOfCommunity(Guid communityId, QueryObject query);
        Task<List<Post>> GetAllPostsOfCommunity(Guid communityId);
        Task<List<Member>> GetAllFollowersOfCommunity(Guid communityId);
        Task<bool> CommunityExists(Guid communityId);
        Task<Community?> GetCommunityById(Guid communityId);
        bool Create(Community community);
        bool Update(Community community);
        bool Delete(Community community);
        bool Save();
    }
}
