using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;

        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool LikePost(string memberID, int communityId, int postId)
        {
            var like = new Like
            {
                CommunityId = communityId,
                PostId = postId,
                MemberId = memberID,
                Time = DateTime.Now
            };
            _context.Add(like); // check if already liked
            return Save();
        }

        public async Task<bool> RemoveAllLikedOfPost(int communityId, int postId)
        {
            var likeds = await _context.Likes.Where(l => l.CommunityId == communityId && l.PostId == postId).ToListAsync();
            foreach (var like in likeds)
            {
                _context.Remove(like);
            }
            return Save();
        }

        public bool LikeComment(string memberID, int communityId, int postId, int commentId)
        {
            var like = new LikeComment
            {
                CommunityId = communityId,
                PostId = postId,
                CommentId = commentId,
                MemberId = memberID,
                Time = DateTime.Now
            };
            _context.Add(like); // check if already liked
            return Save();
        }

        public Task<bool> RemoveAllLikedOfComment(int communityId, int postId, int commentId)
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
