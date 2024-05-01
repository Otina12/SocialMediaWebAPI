using MediatR;
using SocialMedia.Application.Dtos.DtosForGet;
using SocialMedia.Application.Helpers.Mappers;
using SocialMedia.Domain.Interfaces;

namespace SocialMedia.Application.Posts.Queries.GetAllPosts;

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

