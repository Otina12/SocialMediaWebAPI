using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.Core.IConfiguration;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Features.Posts.Commands;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Features.Posts.Handlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public CreatePostCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = request.PostDto.MapToPost();

            post.Id = Guid.NewGuid();
            post.MemberId = request.UserId;
            post.CommunityId = request.CommunityId;

            await _unitOfWork.Posts.Add(post);
            await _unitOfWork.SaveChangesAsync();

            return post.MapToPostDto();
        }
    }
}
