﻿using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Services
{
    public class StaticWrapperService : IStaticWrapper
    {
        public PostDto ToPostDto(Post post)
        {
            return post.MapToPostDto();
        }
    }
}