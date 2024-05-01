using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialMedia.Domain.Entites;
using SocialMedia.Domain.Interfaces;

namespace SocialMedia.Persistence.Repositories
{
    public class CommunityRepository : GenericRepository<Community>, ICommunityRepository
    {

        public CommunityRepository(ApplicationDbContext context, ILogger logger) : base(context, logger)
        {

        }


        public async Task<bool> CommunityExists(Guid communityId)
        {
            var community = await dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == communityId);
            return community != null;
        }

        public async Task<IEnumerable<Community>> CommunitiesOfMember(string memberId)
        {
            var communities = await dbSet
                .AsNoTracking()
                .Where(c => c.CreatorId == memberId)
                .ToListAsync();

            return communities;
        }

        public async Task<List<Community>> GetAllFollowingsOfMember(string memberId)
        {
            var communities = await _context.Followings.Where(f => f.FollowerId == memberId).Select(f => f.Community).ToListAsync();
            return communities;
        }

        public async override Task<IEnumerable<Community>> GetAllAsync()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} method error", typeof(CommunityRepository));
                return new List<Community>();
            }
        }

        public async override Task<Community?> GetByIdAsync(Guid communityId)
        {
            var community = await dbSet
                .FirstOrDefaultAsync(c => c.Id == communityId);
            return community;
        }

        public async override Task<bool> Update(Community community)
        {
            try
            {
                var existingCommunity = await dbSet.FirstOrDefaultAsync(x => x.Id == community.Id);

                if (existingCommunity is null)
                {
                    return await Add(community);
                }

                existingCommunity.Name = community.Name;
                existingCommunity.Description = community.Description;
                existingCommunity.MemberCount = community.MemberCount;
                existingCommunity.PfpUrl = community.PfpUrl;
                existingCommunity.MemberCount = community.MemberCount;

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update method error", typeof(CommunityRepository));
                return false;
            }
        }

        public async override Task<bool> Delete(Guid id)
        {
            try
            {
                var exist = await dbSet.FirstOrDefaultAsync(x => x.Id == id);

                if (exist is not null)
                {
                    dbSet.Remove(exist);
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Repo} Update method error", typeof(CommunityRepository));
                return false;
            }
        }
    }
}
