﻿using MediatR;
using SocialMediaWebApp.DTOs;

namespace SocialMediaWebApp.Features.Posts.Queries
{
    public sealed record GetAllPostsQuery() : IRequest<IEnumerable<PostDto>>;
}