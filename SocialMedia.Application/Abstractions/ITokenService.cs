using SocialMedia.Domain.Entites;

namespace SocialMedia.Application.Abstractions
{
    public interface ITokenService
    {
        string CreateToken(Member member);
    }
}
