﻿using AutoMapper.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.Dtos.DtosForGet;
using SocialMedia.Application.Dtos.DtosForPost;
using SocialMedia.Application.Helpers.Extensions;
using SocialMedia.Application.Helpers.Mappers;
using SocialMedia.Domain.Interfaces;
using SocialMedia.Domain.Primitives;
using System.Security.Claims;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : BaseController
    {

        public CommunityController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext) : base(unitOfWork, httpContext)
        {
        }


        [HttpGet("{communityId}")]
        public async Task<ActionResult<List<PostDto>>> GetCommunity([FromRoute] Guid communityId)
        {
            var community = await _unitOfWork.Communities.GetByIdAsync(communityId);

            if(community is null)
            {
                return NotFound();
            }

            return Ok(community);
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

            var followers = await _unitOfWork.Members.GetAllFollowersOfCommunity(communityId);
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

            await _unitOfWork.SaveChangesAsync();

            return Ok(community);
        }


        [HttpDelete]
        [Authorize]
        [Route("{communityId}/Delete")]
        public async Task<IActionResult> DeleteCommunity([FromRoute] Guid communityId)
        {
            var community = await _unitOfWork.Communities.GetByIdAsync(communityId);

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


            var followings = await _unitOfWork.Followings.GetFollowingsOfCommunity(communityId);
            foreach (var following in followings)
            {
                _unitOfWork.Followings.Unfollow(following);
            }

            await _unitOfWork.Communities.Delete(communityId);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
