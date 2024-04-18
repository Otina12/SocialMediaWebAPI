using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Helpers;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;
using SocialMediaWebApp.Extensions;
using System.Security.Claims;
using SocialMediaWebApp.Core.IRepositories;
using SocialMediaWebApp.Core.IConfiguration;

namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFollowRepository _followRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IHttpContextAccessor _httpContext;

        public CommunityController(IUnitOfWork unitOfWork, IMemberRepository memberRepository, IFollowRepository followRepository, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _memberRepository = memberRepository;
            _followRepository = followRepository;
            _httpContext = httpContext;
        }

        [HttpGet("{communityId}/Posts")]
        public async Task<ActionResult<List<PostDto>>> GetAllPostsOfCommunity([FromRoute] Guid communityId, [FromQuery] QueryObject queryObject)
        {
            var posts = await _unitOfWork.Posts.GetAllPostsOfCommunity(communityId, queryObject);
            var postDtos = posts.Select(p => p.MapToPostDto());

            return Ok(postDtos);
        }

        [HttpGet("{communityId}/Followers")]
        public async Task<ActionResult<List<Member>>> GetAllFollowersOfCommunity([FromRoute] Guid communityId)
        {
            var communityExists = await _unitOfWork.Communities.CommunityExists(communityId);

            if (!communityExists)
            {
                ModelState.AddModelError("404", "Community was not found");
                return BadRequest(ModelState);
            }

            var followers = await _memberRepository.GetAllFollowersOfCommunity(communityId);
            var followersDto = followers.Select(f => f.MapToFollowerDto());

            return Ok(followersDto);
        }


        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityDto communityDto)
        {
            var community = communityDto.MapToCommunity();
            community.CreatorId = _httpContext.HttpContext!.User.GetCurrentUserId();

            var created = await _unitOfWork.Communities.Add(community);

            if (!created)
            {
                ModelState.AddModelError("500", "Something went wrong while creating a community");
                return BadRequest(ModelState);
            }

            await _unitOfWork.CompleteAsync();

            return Ok(community);
        }


        [HttpDelete]
        [Authorize]
        [Route("{communityId}/Delete")]
        public async Task<IActionResult> DeleteCommunity([FromRoute] Guid communityId)
        {
            var community = await _unitOfWork.Communities.GetById(communityId);

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

            var posts = await _unitOfWork.Posts.GetAllPostsOfCommunity(communityId);
            foreach (var post in posts)
            {
                var comments = await _unitOfWork.Comments.GetAllCommentsOfPost(post.Id);
                foreach (var comment in comments)
                {
                    await _unitOfWork.Comments.Delete(comment.Id);
                }
                await _unitOfWork.Posts.Delete(post.Id);
            }


            var followings = await _followRepository.GetFollowingsOfCommunity(communityId);
            foreach (var following in followings)
            {
                _followRepository.Unfollow(following);
            }

            await _unitOfWork.Communities.Delete(communityId);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
