using MediatR;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Features.Posts.Commands
{
    public sealed record CreatePostCommand(Guid CommunityId, string UserId, CreatePostDto PostDto) : IRequest<PostDto>;
}
