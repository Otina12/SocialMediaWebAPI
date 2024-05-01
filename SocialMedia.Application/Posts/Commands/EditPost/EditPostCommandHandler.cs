using MediatR;
using SocialMedia.Application.Dtos.DtosForGet;
using SocialMedia.Application.Helpers.Mappers;
using SocialMedia.Domain.Interfaces;


namespace SocialMedia.Application.Posts.Commands.EditPost
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
