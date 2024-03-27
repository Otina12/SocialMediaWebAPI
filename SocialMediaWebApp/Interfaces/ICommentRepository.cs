using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentsOfPost(int communityId, int postId);
        Task<Comment?> GetCommentById(int communityId, int postId, int commentId);
        Task<List<Comment>> GetAllCommentsOfMember(string memberId);
        Task<List<Comment>> GetAllRepliesOfAComment(int communityId, int postId, int commentId);
        int GetFirstAvailableId(int communityId, int postId);
        Task<bool> CommentExists(int communityId, int postId, int commentId);
        bool Create(Comment comment);
        bool Update(Comment comment);
        bool Delete(Comment comment);
        bool Save();
    }
}
