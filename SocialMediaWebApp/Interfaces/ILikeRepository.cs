namespace SocialMediaWebApp.Interfaces
{
    public interface ILikeRepository
    {
        bool LikePost(string memberID, Guid communityId, Guid postId);
        Task<bool> RemoveAllLikedOfPost(Guid communityId, Guid postId);
        bool LikeComment(string memberID, Guid communityId, Guid postId, Guid commentId);
        Task<bool> RemoveAllLikedOfComment(Guid communityId, Guid postId, Guid commentId);
        bool Save();
    }
}
