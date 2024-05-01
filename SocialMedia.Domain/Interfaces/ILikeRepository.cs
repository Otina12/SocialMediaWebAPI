
namespace SocialMedia.Domain.Interfaces
{
    public interface ILikeRepository
    {
        bool LikePost(string memberID, Guid postId);
        Task<bool> RemoveAllLikedOfPost(Guid postId);
        bool LikeComment(string memberID, Guid commentId);
        Task<bool> RemoveAllLikedOfComment(Guid commentId);
    }
}
