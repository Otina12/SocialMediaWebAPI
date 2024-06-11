using MediatR;
using SocialMedia.Application.Abstractions.Messaging;
using SocialMedia.Application.Dtos.DtosForGet;
using SocialMedia.Application.Dtos.DtosForPost;
using SocialMedia.Domain.Shared;
using System.Windows.Input;

namespace SocialMedia.Application.Posts.Commands.CreatePost;

public sealed record CreatePostCommand(Guid CommunityId, string UserId, CreatePostDto PostDto) : IRequest<Result<PostDto>>;
