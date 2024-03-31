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
    public class PostControllerTests
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommunityRepository _communityRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextAccessor _httpContext;

        public PostControllerTests()
        {
            _postRepository = A.Fake<IPostRepository>();
            _commentRepository = A.Fake<ICommentRepository>();
            _communityRepository = A.Fake<ICommunityRepository>();
            _likeRepository = A.Fake<ILikeRepository>();
            _httpContext = A.Fake<IHttpContextAccessor>();
        }

        [Fact]
        public void PostController_GetPostById_ReturnOk()
        {
            int communityId = 1, postId = 1;
            var post = A.Fake<Post>();
            var postDto = A.Fake<PostDto>();
            var controller = new PostController(_postRepository, _commentRepository, _communityRepository, _likeRepository, _httpContext);
            A.CallTo(() => PostMapper.MapToPostDto(post)).Returns(postDto);

            A.CallTo(() => _postRepository.GetPostByIdAsync(communityId, postId)).Returns(Task.FromResult(post));
            var result = post.MapToPostDto();
            result.Should().BeOfType(typeof(OkObjectResult));
        }
    }
}