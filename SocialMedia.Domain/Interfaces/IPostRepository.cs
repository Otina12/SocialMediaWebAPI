using SocialMedia.Domain.Entites;
using SocialMedia.Domain.Primitives;

namespace SocialMedia.Domain.Interfaces
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetAllPostsOfCommunity(Guid communityId, QueryObject query);
        Task<List<Post>> GetAllPostsOfCommunity(Guid communityId);
        Task<List<Post>?> GetAllPostsOfMemberAsync(string memberId);
        Task<bool> PostExists(Guid PostId);
    }
}
