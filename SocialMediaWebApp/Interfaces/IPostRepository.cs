using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<List<Post>?> GetAllPostsOfMemberAsync(string memberId);
        Task<Post?> GetPostByIdAsync(int CommunityId, int PostId);
        int GetFirstAvailableId(int communityID); // this will get the first available Id for creating the post
        Task<bool> PostExists(int communityId, int postId);
        bool Create(Post post);
        bool Update(Post post);
        bool Delete(Post post);
        bool Save();
    }
}
