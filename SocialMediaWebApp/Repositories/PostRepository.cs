using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            var posts = await _context.Posts.ToListAsync();
            return posts;
        }

        public async Task<List<Post>?> GetAllPostsOfMemberAsync(string memberId)
        {
            var posts = await _context.Posts.Where(p => p.MemberId == memberId).ToListAsync();
            return posts;
        }

        public async Task<Post?> GetPostByIdAsync(Guid communityId, Guid postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.CommunityId == communityId && p.Id == postId);
            return post;
        }

        public async Task<bool> PostExists(Guid communityId, Guid postId)
        {
            var post = await _context.Posts.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CommunityId == communityId && c.Id == postId);
            return post != null;
        }

        public bool Create(Post post)
        {
            _context.Add(post);
            return Save();
        }

        public bool Update(Post post)
        {
            post.IsEdited = true;
            post.EditTime = DateTime.Now;
            _context.Update(post);
            return Save();
        }

        public bool Delete(Post post)
        {
            _context.Remove(post);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
