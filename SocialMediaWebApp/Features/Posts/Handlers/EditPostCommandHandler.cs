using MediatR;
using SocialMediaWebApp.Core.IConfiguration;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Features.Posts.Commands;
using SocialMediaWebApp.Mappers;

namespace SocialMediaWebApp.Features.Posts.Handlers
{
    public class EditPostCommandHandler : IRequestHandler<EditPostCommand, PostDto>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public EditPostCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PostDto> Handle(EditPostCommand request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(request.PostId);

            post!.Content = request.postDto.Content;
            post!.IsEdited = true;
            post!.EditTime = DateTime.Now;

            await _unitOfWork.Posts.Update(post);
            await _unitOfWork.SaveChangesAsync();

            return post.MapToPostDto();
        }
    }
}
