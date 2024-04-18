using SocialMediaWebApp.Helpers;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Core.IRepositories
{
    public interface ICommunityRepository : IGenericRepository<Community>
    {
        Task<bool> CommunityExists(Guid communityId);
        Task<IEnumerable<Community>> CommunitiesOfMember(string memberId);
    }
}
