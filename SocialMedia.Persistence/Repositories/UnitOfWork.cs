﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialMedia.Domain.Interfaces;


namespace SocialMedia.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public ICommunityRepository Communities { get; private set; }

        public IPostRepository Posts { get; private set; }

        public ICommentRepository Comments { get; private set; }

        public IFollowRepository Followings { get; private set; }

        public ILikeRepository Likes { get; private set; }

        public IMemberRepository Members { get; private set; }


        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Communities = new CommunityRepository(_context, _logger);
            Posts = new PostRepository(_context, _logger);
            Comments = new CommentRepository(_context, _logger);
            Followings = new FollowRepository(_context);
            Likes = new LikeRepository(_context);
            Members = new MemberRepository(_context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
