using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Core.IRepositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetAllCommentsOfPost(Guid postId);
        Task<Comment?> GetCommentById(Guid commentId);
        Task<List<Comment>> GetAllCommentsOfMember(string memberId);
        Task<List<Comment>> GetAllRepliesOfAComment(Guid commentId);
        Task<bool> CommentExists(Guid commentId);
    }
}
