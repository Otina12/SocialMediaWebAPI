using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Core.IRepositories
{
    public interface ITokenService
    {
        string CreateToken(Member member);
    }
}
