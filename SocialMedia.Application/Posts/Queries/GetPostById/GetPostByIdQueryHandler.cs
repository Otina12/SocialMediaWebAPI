using MediatR;
using SocialMedia.Application.Dtos.DtosForGet;
using SocialMedia.Application.Helpers.Mappers;
using SocialMedia.Domain.Interfaces;
namespace SocialMedia.Application.Posts.Queries.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDto>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public GetPostByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(request.DriverId);

            if (post is null)
            {
                return null;
            }

            return post.MapToPostDto();
        }
    }
}
