using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Entites;

namespace SocialMedia.Domain.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetAllCommentsOfPost(Guid postId);
        Task<List<Comment>> GetAllCommentsOfMember(string memberId);
        Task<List<Comment>> GetAllRepliesOfAComment(Guid commentId);
        Task<bool> CommentExists(Guid commentId);
    }
}
