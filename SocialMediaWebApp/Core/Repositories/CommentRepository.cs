using Microsoft.EntityFrameworkCore;
using SocialMediaWebApp.Core.IRepositories;
using SocialMediaWebApp.Data;
using SocialMediaWebApp.Exceptions;
using SocialMediaWebApp.Models;
using System.Xml.Linq;

namespace SocialMediaWebApp.Core.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {

        }

        public async Task<List<Comment>> GetAllCommentsOfPost(Guid postId)
        {
            var post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(x => x.Id == postId);
            
            if (post == null)
                throw new PostNotFoundException($"Post with Id = {postId} was not found");

            var comments = post.Comments.ToList();
            return comments;
        }

        public async Task<List<Comment>> GetAllCommentsOfMember(string memberId)
        {
            var comments = await dbSet
                .Where(c => c.MemberId == memberId).ToListAsync();
            return comments;
        }

        public async Task<List<Comment>> GetAllRepliesOfAComment(Guid commentId)
        {
            var replies = await dbSet
                .Where(c => c.IsReplyToId == commentId).ToListAsync();
            return replies;
        }

        public async Task<bool> CommentExists(Guid commentId)
        {
            var comment = await dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == commentId);
            return comment != null;
        }

        public async override Task<bool> Update(Comment comment)
        {
            var existingComment = await dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == comment.Id);

            if(existingComment is null)
            {
                return false;
            }

            dbSet.Update(comment);
            return true;
        }
    }
}
