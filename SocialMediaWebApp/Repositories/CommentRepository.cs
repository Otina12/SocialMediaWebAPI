using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Models;
using System.Xml.Linq;

namespace SocialMediaWebApp.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllCommentsOfPost(int communityId, int postId)
        {
            var comments = await _context.Comments.Where(c => c.PostId == postId && c.CommunityId == communityId).ToListAsync();
            return comments;
        }

        public async Task<Comment?> GetCommentById(int communityId, int postId, int commentId)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId && c.PostId == postId && c.CommunityId == communityId);
            return comment;
        }

        public async Task<List<Comment>> GetAllCommentsOfMember(string memberId)
        {
            var comments = await _context.Comments.Where(c => c.MemberId == memberId).ToListAsync();
            return comments;
        }

        public async Task<List<Comment>> GetAllRepliesOfAComment(int communityId, int postId, int commentId)
        {
            var replies = await _context.Comments.Where(c => c.IsReplyToId == commentId && c.PostId == postId && c.CommunityId == communityId).ToListAsync();
            return replies;
        }

        public int GetFirstAvailableId(int communityId, int postId)
        {
            var comments = _context.Comments.AsNoTracking()
                .Where(c => c.CommunityId == communityId && c.PostId == postId).ToList();
            if(comments.Count == 0)
            {
                return 1;
            }
            int lastId = comments.Last().Id;
            return lastId + 1;
        }

        public async Task<bool> CommentExists(int communityId, int postId, int commentId)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId && c.CommunityId == communityId && c.PostId == postId);
            return comment != null;
        }

        public bool Create(Comment comment)
        {
            _context.Add(comment);
            return Save();
        }

        public bool Update(Comment comment)
        {
            comment.IsEdited = true;
            comment.EditTime = DateTime.Now;
            _context.Update(comment);
            return Save();
        }

        public bool Delete(Comment comment)
        {
            _context.Remove(comment);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
