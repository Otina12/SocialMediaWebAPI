using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Helpers;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Repositories
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ApplicationDbContext _context;

        public CommunityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Community?> GetCommunityById(Guid communityId)
        {
            var community = await _context.Communities.FirstOrDefaultAsync(c => c.Id == communityId);
            return community;
        }

        public async Task<List<Post>> GetAllPostsOfCommunity(Guid communityId, QueryObject query)
        {
            var posts = _context.Posts.Where(p => p.CommunityId == communityId).AsQueryable();

            if (!query.NewestFirst)
            {
                posts = posts.OrderBy(p => p.PostTime);
            }
            else
            {
                posts = posts.OrderByDescending(p => p.PostTime);
            }

            var skipCount = (query.PageNumber - 1) * query.PageSize;
            var finalPosts = await posts.Skip(skipCount).Take(query.PageSize).ToListAsync();
            return finalPosts;
        }

        public async Task<List<Post>> GetAllPostsOfCommunity(Guid communityId)
        {
            var posts = await _context.Posts.Where(p => p.CommunityId == communityId).ToListAsync();
            return posts;
        }

        public async Task<List<Member>> GetAllFollowersOfCommunity(Guid communityId)
        {
            var members = await _context.Followings
                .Where(f => f.CommunityId == communityId)
                .Select(f => f.Follower)
                .ToListAsync();
            return members;
        }

        public async Task<bool> CommunityExists(Guid communityId)
        {
            var community = await _context.Communities.AsNoTracking().FirstOrDefaultAsync(c => c.Id == communityId);
            return community != null;
        }


        public bool Create(Community community)
        {
            _context.Add(community);
            return Save();
        }

        public bool Update(Community community)
        {
            _context.Update(community);
            return Save();
        }
        public bool Delete(Community community)
        {
            _context.Remove(community);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
