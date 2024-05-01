using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialMedia.Application.Dtos;
using SocialMedia.Domain.Entites;
using SocialMedia.Domain.Exceptions;
using SocialMedia.Domain.Interfaces;
using SocialMedia.Domain.Primitives;

namespace SocialMedia.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {

        public PostRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {

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

        public async Task<List<Post>> GetAllPostsOfCommunity(Guid communityId, QueryObject query)
        {
            var community = await _context.Communities.Include(c => c.Posts).FirstOrDefaultAsync(x => x.Id ==  communityId);

            if(community is null)
            {
                throw new CommunityNotFoundException($"Community with Id = {communityId} was not found");
            }

            var posts = community.Posts;

            if (!query.NewestFirst)
            {
                posts = posts.OrderBy(p => p.PostTime).ToList();
            }
            else
            {
                posts = posts.OrderByDescending(p => p.PostTime).ToList();
            }

            var skipCount = (query.PageNumber - 1) * query.PageSize;
            var finalPosts = posts.Skip(skipCount).Take(query.PageSize).ToList();

            return finalPosts;
        }

        public async Task<List<Post>> GetAllPostsOfCommunity(Guid communityId)
        {
            var community = await _context.Communities.Include(c => c.Posts).FirstOrDefaultAsync(x => x.Id == communityId);

            if (community is null)
            {
                throw new CommunityNotFoundException($"Community with Id = {communityId} was not found");
            }

            return community.Posts.ToList();
        }

        public async Task<bool> PostExists(Guid postId)
        {
            var post = await dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == postId);

            return post != null;
        }

        public async override Task<bool> Update(Post entity)
        {
            try
            {
                var existingPost = await dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (existingPost is null)
                {
                    return await Add(entity);
                }

                existingPost.Content = entity.Content;
                existingPost.LikeCount = entity.LikeCount;
                existingPost.CommentCount = entity.CommentCount;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update method error", typeof(PostRepository));
                return false;
            }
        }
    }
}
