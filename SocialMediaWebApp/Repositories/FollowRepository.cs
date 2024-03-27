using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Repositories
{
    public class FollowRepository : IFollowRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Following>> GetFollowingsOfMember(string memberId)
        {
            var followings = await _context.Followings.Where(f => f.FollowerId == memberId).ToListAsync();
            return followings;
        }

        public async Task<List<Following>> GetFollowingsOfCommunity(int communityId)
        {
            var followings = await _context.Followings.Where(f => f.CommunityId == communityId).ToListAsync();
            return followings;
        }

        public async Task<Following?> GetFollowingById(string memberId, int communityId)
        {
            var following = await _context.Followings.FirstOrDefaultAsync(f => f.FollowerId == memberId && f.CommunityId == communityId);
            return following;
        }

        public bool Follow(string memberId, int communityId)
        {
            var followingExists = _context.Followings.AsNoTracking().Any(f => f.FollowerId == memberId && f.CommunityId == communityId);
            if(followingExists)
            {
                return false;
            }

            var following = new Following { FollowerId = memberId, CommunityId = communityId };
            _context.Add(following);
            return Save();
        }

        public bool Unfollow(Following following)
        {
            _context.Remove(following);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

    }
}
