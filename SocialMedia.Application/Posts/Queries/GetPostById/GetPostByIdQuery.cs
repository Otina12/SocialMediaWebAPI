using MediatR;
using SocialMedia.Application.Dtos.DtosForGet;

namespace SocialMedia.Application.Posts.Queries.GetPostById
{
    public sealed record GetPostByIdQuery(Guid DriverId) : IRequest<PostDto>;

}
