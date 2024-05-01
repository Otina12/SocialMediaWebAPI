
namespace SocialMedia.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        ICommunityRepository Communities { get; }
        IPostRepository Posts { get; }
        ICommentRepository Comments { get; }
        IFollowRepository Followings { get; }
        ILikeRepository Likes { get; }
        IMemberRepository Members { get; }

        Task SaveChangesAsync();
    }
}
