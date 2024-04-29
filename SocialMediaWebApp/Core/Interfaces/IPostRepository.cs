using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Helpers;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Core.IRepositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetAllPostsOfCommunity(Guid communityId, QueryObject query);
        Task<List<Post>> GetAllPostsOfCommunity(Guid communityId);
        Task<List<Post>?> GetAllPostsOfMemberAsync(string memberId);
        Task<bool> PostExists(Guid PostId);
    }
}
