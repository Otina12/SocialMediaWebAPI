using MediatR;
using SocialMediaWebApp.Core.IConfiguration;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Features.Posts.Queries;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Features.Posts.Handlers
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
