using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.Controllers;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaWebApp.Tests.ControllerTest
{
    public class PostControllerTest
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommunityRepository _communityRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextAccessor _httpContext;

        public PostControllerTest()
        {
            _postRepository = A.Fake<IPostRepository>();
            _commentRepository = A.Fake<ICommentRepository>();
            _communityRepository = A.Fake<ICommunityRepository>();
            _likeRepository = A.Fake<ILikeRepository>();
            _httpContext = A.Fake<IHttpContextAccessor>();
        }

        [Fact]
        public async void PostController_GetPostById_ReturnOk()
        {
            var post = A.Fake<Post>();
            var postDto = A.Fake<PostDto>();
            var controller = new PostController(_postRepository, _commentRepository, _communityRepository, _likeRepository, _httpContext);

            A.CallTo(() => PostMapper.MapToPostDto(post)).Returns(postDto);

            var result = controller.GetPostById(0, 1);

            result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}