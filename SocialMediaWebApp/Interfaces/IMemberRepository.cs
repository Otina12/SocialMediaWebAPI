using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member?> GetMemberById(string id);
        Task<bool> MemberExists(string memberId);
        Task<List<Community>> GetAllFollowingsOfMember(string memberId);
        bool Update(Member member);
        bool Save();
    }
}
