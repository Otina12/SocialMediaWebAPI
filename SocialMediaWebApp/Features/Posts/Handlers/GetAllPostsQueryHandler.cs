using MediatR;
using SocialMediaWebApp.Core.IConfiguration;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Features.Posts.Queries;
using SocialMediaWebApp.Mappers;

namespace SocialMediaWebApp.Features.Posts.Handlers
{
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, IEnumerable<PostDto>>
    {
        protected readonly IUnitOfWork _unitOfWork;

        public GetAllPostsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.Posts.GetAllAsync();
            var postDtos = posts.Select(p => p.MapToPostDto());

            return postDtos;
        }
    }
}
