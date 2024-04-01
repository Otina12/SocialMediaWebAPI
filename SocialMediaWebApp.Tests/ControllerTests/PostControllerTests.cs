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
        private readonly IStaticWrapper _staticWrapper;

        public PostControllerTests()
        {
            _postRepository = A.Fake<IPostRepository>();
            _commentRepository = A.Fake<ICommentRepository>();
            _communityRepository = A.Fake<ICommunityRepository>();
            _likeRepository = A.Fake<ILikeRepository>();
            _httpContext = A.Fake<IHttpContextAccessor>();
            _staticWrapper = A.Fake<IStaticWrapper>();
        }

        [Fact]
        public async void GetPostById_ReturnOk_WhenPostFound()
        {
            int communityId = 1, postId = 1;
            var post = A.Fake<Post>();
            var postDto = A.Fake<PostDto>();
            var controller = new PostController(_postRepository, _commentRepository, _communityRepository, _likeRepository, _httpContext);
            
            A.CallTo(() => _postRepository.GetPostByIdAsync(communityId, postId))!.Returns(Task.FromResult(post));
            A.CallTo(() => _staticWrapper.ToPostDto(post)).Returns(postDto);

            var result = await controller.GetPostById(communityId, postId);

            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<PostDto>>();
        }
    }
}