namespace SocialMediaWebApp.Core.IRepositories
{
    public interface ILikeRepository
    {
        bool LikePost(string memberID, Guid postId);
        Task<bool> RemoveAllLikedOfPost(Guid postId);
        bool LikeComment(string memberID, Guid commentId);
        Task<bool> RemoveAllLikedOfComment(Guid commentId);
    }
}
