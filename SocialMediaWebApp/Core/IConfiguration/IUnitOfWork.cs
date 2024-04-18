using SocialMediaWebApp.Interfaces;

namespace SocialMediaWebApp.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        ICommentRepository Comments { get; }

        Task CompleteAsync();
    }
}
