using MediatR;
using SocialMediaWebApp.DTOs;

namespace SocialMediaWebApp.Features.Posts.Commands;

public sealed record EditPostCommand(Guid PostId, string UserId, CreatePostDto postDto) : IRequest<PostDto>;

