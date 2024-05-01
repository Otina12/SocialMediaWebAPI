using MediatR;
using SocialMedia.Application.Abstractions.Messaging;
using SocialMedia.Application.Dtos.DtosForGet;
using SocialMedia.Application.Helpers.Mappers;
using SocialMedia.Domain.Interfaces;
using SocialMedia.Domain.Shared;

namespace SocialMedia.Application.Posts.Commands.CreatePost
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

        //public async Task<Result<PostDto>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        //{
        //    var post = command.PostDto.MapToPost();

        //    post.Id = Guid.NewGuid();
        //    post.MemberId = command.UserId;
        //    post.CommunityId = command.CommunityId;

        //    await _unitOfWork.Posts.Add(post);
        //    await _unitOfWork.SaveChangesAsync();

        //    return post.MapToPostDto();
        //}
    }
}
