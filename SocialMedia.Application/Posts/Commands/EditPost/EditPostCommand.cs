using MediatR;
using SocialMedia.Application.Dtos.DtosForGet;
using SocialMedia.Application.Dtos.DtosForPost;

namespace SocialMedia.Application.Posts.Commands.EditPost;

public sealed record EditPostCommand(Guid PostId, string UserId, CreatePostDto postDto) : IRequest<PostDto>;

