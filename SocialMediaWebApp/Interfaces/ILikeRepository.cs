namespace SocialMediaWebApp.Interfaces
{
    public interface ILikeRepository
    {
        bool LikePost(string memberID, int communityId, int postId);
        Task<bool> RemoveAllLikedOfPost(int communityId, int postId);
        bool LikeComment(string memberID, int communityId, int postId, int commentId);
        Task<bool> RemoveAllLikedOfComment(int communityId, int postId, int commentId);
        bool Save();
    }
}
