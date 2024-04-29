using MediatR;
using SocialMediaWebApp.DTOs;

namespace SocialMediaWebApp.Features.Posts.Queries
{
    public sealed record GetPostByIdQuery(Guid DriverId) : IRequest<PostDto>;

}
