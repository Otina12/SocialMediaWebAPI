using SocialMedia.Domain.Entites;

namespace SocialMedia.Domain.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member?> GetMemberById(string id);
        Task<bool> MemberExists(string memberId);
        Task<List<Member>> GetAllFollowersOfCommunity(Guid communityId);
        bool Update(Member member);
    }
}
