using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentsOfPost(Guid communityId, Guid postId);
        Task<Comment?> GetCommentById(Guid communityId, Guid postId, Guid commentId);
        Task<List<Comment>> GetAllCommentsOfMember(string memberId);
        Task<List<Comment>> GetAllRepliesOfAComment(Guid communityId, Guid postId, Guid commentId);
        Task<bool> CommentExists(Guid communityId, Guid postId, Guid commentId);
        bool Create(Comment comment);
        bool Update(Comment comment);
        bool Delete(Comment comment);
        bool Save();
    }
}
