using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Helpers;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Repositories;
using System.Security.Claims;

namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IFollowRepository _followRepository;
        private readonly IHttpContextAccessor _httpContext;

        public CommunityController(ICommunityRepository communityRepository, IPostRepository postRepository,
            ICommentRepository commentRepository, IFollowRepository followRepository, IHttpContextAccessor httpContext)
        {
            _communityRepository = communityRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _followRepository = followRepository;
            _httpContext = httpContext;
        }

        [HttpGet("{communityId}/Posts")]
        public async Task<ActionResult<List<PostDto>>> GetAllPostsOfCommunity([FromRoute] Guid communityId, [FromQuery] QueryObject queryObject)
        {
            var posts = await _communityRepository.GetAllPostsOfCommunity(communityId, queryObject);
            var postDtos = posts.Select(p => p.MapToPostDto());

            return Ok(postDtos);
        }

        [HttpGet("{communityId}/Followers")]
        public async Task<ActionResult<List<Member>>> GetAllFollowersOfCommunity([FromRoute] Guid communityId)
        {
            var communityExists = await _communityRepository.CommunityExists(communityId);

            if (!communityExists)
            {
                ModelState.AddModelError("404", "Community was not found");
                return BadRequest(ModelState);
            }

            var followers = await _communityRepository.GetAllFollowersOfCommunity(communityId);
            var followersDto = followers.Select(f => f.MapToFollowerDto());

            return Ok(followersDto);
        }


        [HttpPost("Create")]
        [Authorize]
        public IActionResult CreateCommunity([FromBody] CreateCommunityDto communityDto)
        {
            var community = communityDto.MapToCommunity();
            community.CreatorId = _httpContext.HttpContext!.User.GetCurrentUserId();

            var created = _communityRepository.Create(community);

            if (!created)
            {
                ModelState.AddModelError("500", "Something went wrong while creating a community");
                return BadRequest(ModelState);
            }

            return Ok(community);
        }


        [HttpDelete]
        [Authorize]
        [Route("Delete/{communityId}")]
        public async Task<IActionResult> DeleteCommunity([FromRoute] Guid communityId)
        {
            var community = await _communityRepository.GetCommunityById(communityId);

            if (community == null)
            {
                ModelState.AddModelError("404", "Community was not found");
                return BadRequest(ModelState);
            }

            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            if (curUserId != community.CreatorId)
            {
                return BadRequest("You are not allowed access to this community");
            }

            var posts = await _communityRepository.GetAllPostsOfCommunity(communityId);
            foreach (var post in posts)
            {
                var comments = await _commentRepository.GetAllCommentsOfPost(community.Id, post.Id);
                foreach (var com in comments)
                {
                    _commentRepository.Delete(com);
                }
                _postRepository.Delete(post);
            }


            var followings = await _followRepository.GetFollowingsOfCommunity(communityId);
            foreach (var following in followings)
            {
                _followRepository.Unfollow(following);
            }

            _communityRepository.Delete(community);

            return Ok();
        }
    }
}
