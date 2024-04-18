﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.Core.IConfiguration;
using SocialMediaWebApp.Core.IRepositories;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Mappers;


namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemberRepository _memberRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IFollowRepository _followRepository;
        private readonly IHttpContextAccessor _httpContext;

        public MemberController(IMemberRepository memberRepository, ILikeRepository likeRepository, IUnitOfWork unitOfWork,
            IFollowRepository followRepository, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _memberRepository = memberRepository;
            _likeRepository = likeRepository;
            _followRepository = followRepository;
            _httpContext = httpContext;
        }

        [HttpGet("{memberId}/Posts")]
        public async Task<IActionResult> GetAllPostsOfUser([FromRoute] string memberId)
        {
            var posts = await _unitOfWork.Posts.GetAllPostsOfMemberAsync(memberId);
            return Ok(posts);
        }

        [HttpGet("{memberId}/Comments")]
        public async Task<IActionResult> GetAllCommentsOfUser([FromRoute] string memberId)
        {
            var comments = await _unitOfWork.Comments.GetAllCommentsOfMember(memberId);
            return Ok(comments);
        }


        [HttpPost("Follow/{communityId}")]
        [Authorize]
        public async Task<IActionResult> FollowCommunity([FromRoute] Guid communityId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var communityExists = await _unitOfWork.Communities.CommunityExists(communityId);

            if (!communityExists)
            {
                ModelState.AddModelError("404", "Member or community was not found");
                return BadRequest(ModelState);
            }

            var followed = _followRepository.Follow(curUserId, communityId);
            if (followed)
            {
                var community = await _unitOfWork.Communities.GetById(communityId);
                community!.MemberCount += 1;
                await _unitOfWork.Communities.Update(community);
            }

            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPost("Unfollow/{communityId}")]
        [Authorize]
        public async Task<IActionResult> UnFollowCommunity([FromRoute] Guid communityId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var communityExists = await _unitOfWork.Communities.CommunityExists(communityId);

            if (!communityExists)
            {
                ModelState.AddModelError("404", "Member or community was not found");
                return BadRequest(ModelState);
            }

            var following = await _followRepository.GetFollowingById(curUserId, communityId);
            if(following is null)
            {
                ModelState.AddModelError("404", "Member was not followed to a community");
                return BadRequest(ModelState);
            }

            var followed = _followRepository.Unfollow(following!);
            if (followed)
            {
                var community = await _unitOfWork.Communities.GetById(communityId);
                community!.MemberCount -= 1;
                await _unitOfWork.Communities.Update(community);
            }

            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpPost("{memberId}/Like/{postId}")]
        [Authorize]
        public async Task<IActionResult> LikePost([FromRoute] string memberId, [FromRoute] Guid postId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var postExists = await _unitOfWork.Posts.PostExists(postId);

            if (!postExists)
            {
                ModelState.AddModelError("404", "Member of post was not found");
                return BadRequest(ModelState);
            }

            var liked = _likeRepository.LikePost(curUserId, postId);
            if (liked)
            {
                var post = await _unitOfWork.Posts.GetById(postId);
                post!.LikeCount += 1;
            }

            await _unitOfWork.CompleteAsync();

            return Ok();
        }


        [HttpPost]
        [Authorize]
        [Route("Like/{commentId}")]
        public async Task<IActionResult> LikeComment([FromRoute] Guid commentId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var postExists = await _unitOfWork.Comments.CommentExists(commentId);

            if (!postExists)
            {
                ModelState.AddModelError("404", "Member of post was not found");
                return BadRequest(ModelState);
            }

            _likeRepository.LikeComment(curUserId, commentId);
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpGet("Followings")]
        [Authorize]
        public async Task<ActionResult<List<CommunityDto>>> GetAllFollowingsOfMember()
        {
            var memberId = _httpContext.HttpContext?.User.GetCurrentUserId();
            var communities = await _memberRepository.GetAllFollowingsOfMember(memberId!);

            var communitiesDto = communities.Select(c => c.MapToCommunityDto());

            return Ok(communitiesDto);
        }

        [HttpPatch("Edit")]
        [Authorize]
        public async Task<IActionResult> EditProfile([FromBody] MemberEditDto memberDto)
        {
            var curMemberId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var curMember = await _memberRepository.GetMemberById(curMemberId);

            curMember!.UserName = memberDto.UserName is null ? curMember.UserName : memberDto.UserName;
            curMember!.Bio = memberDto.Bio is null ? curMember.Bio : memberDto.Bio;
            curMember!.ProfilePhotoUrl = memberDto.ProfilePhotoUrl is null ? curMember.ProfilePhotoUrl : memberDto.ProfilePhotoUrl;

            _memberRepository.Update(curMember);
            return Ok(curMember.MapToFollowerDto());
        }
    }
}
