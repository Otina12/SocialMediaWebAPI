using MediatR;
using SocialMedia.Application.Dtos.DtosForGet;

namespace SocialMedia.Application.Posts.Queries.GetAllPosts
{
    public sealed record GetAllPostsQuery() : IRequest<IEnumerable<PostDto>>;
}
