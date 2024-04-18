using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Core.IRepositories
{
    public interface IStaticWrapper
    {
        PostDto ToPostDto(Post post);
    }
}
