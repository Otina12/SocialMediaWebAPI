using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface IStaticWrapper
    {
        PostDto ToPostDto(Post post);
    }
}
