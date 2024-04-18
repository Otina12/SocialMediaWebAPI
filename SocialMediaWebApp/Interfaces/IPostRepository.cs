using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<List<Post>?> GetAllPostsOfMemberAsync(string memberId);
        Task<Post?> GetPostByIdAsync(Guid CommunityId, Guid PostId);
        Task<bool> PostExists(Guid CommunityId, Guid PostId);
        bool Create(Post post);
        bool Update(Post post);
        bool Delete(Post post);
        bool Save();
    }
}
