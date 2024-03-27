using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Member member);
    }
}
