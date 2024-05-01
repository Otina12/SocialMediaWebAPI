using SocialMedia.Domain.Entites;

namespace SocialMedia.Domain.Interfaces
{
    public interface ICommunityRepository : IGenericRepository<Community>
    {
        Task<bool> CommunityExists(Guid communityId);
        Task<IEnumerable<Community>> CommunitiesOfMember(string memberId);
        Task<List<Community>> GetAllFollowingsOfMember(string memberId);
    }
}
