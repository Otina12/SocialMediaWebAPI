using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Core.IRepositories
{
    public interface IMemberRepository
    {
        Task<Member?> GetMemberById(string id);
        Task<bool> MemberExists(string memberId);
        Task<List<Member>> GetAllFollowersOfCommunity(Guid communityId);
        Task<List<Community>> GetAllFollowingsOfMember(string memberId);
        bool Update(Member member);
    }
}
