using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Core.IRepositories;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Core.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;

        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool LikePost(string memberID, Guid postId)
        {
            var like = new Like
            {
                PostId = postId,
                MemberId = memberID,
                Time = DateTime.Now
            };
            _context.Add(like); // check if already liked
            return Save();
        }

        public async Task<bool> RemoveAllLikedOfPost(Guid postId)
        {
            var likeds = await _context.Likes.Where(l => l.PostId == postId).ToListAsync();
            foreach (var like in likeds)
            {
                _context.Remove(like);
            }
            return Save();
        }

        public bool LikeComment(string memberID, Guid commentId)
        {
            var like = new LikeComment
            {
                CommentId = commentId,
                MemberId = memberID,
                Time = DateTime.Now
            };
            _context.Add(like); // check if already liked
            return Save();
        }

        public Task<bool> RemoveAllLikedOfComment(Guid commentId)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
