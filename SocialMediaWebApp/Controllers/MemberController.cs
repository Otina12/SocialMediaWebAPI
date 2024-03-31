using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Mappers;


namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ICommunityRepository _communityRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IFollowRepository _followRepository;
        private readonly IHttpContextAccessor _httpContext;

        public MemberController(IMemberRepository memberRepository, IPostRepository postRepository, ILikeRepository likeRepository,
            ICommunityRepository communityRepository, ICommentRepository commentRepository, IFollowRepository followRepository, IHttpContextAccessor httpContext)
        {
            _memberRepository = memberRepository;
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _communityRepository = communityRepository;
            _commentRepository = commentRepository;
            _followRepository = followRepository;
            _httpContext = httpContext;
        }

        [HttpGet("{memberId}/Posts")]
        public async Task<IActionResult> GetAllPostsOfUser([FromRoute] string memberId)
        {
            var posts = await _postRepository.GetAllPostsOfMemberAsync(memberId);
            return Ok(posts);
        }

        [HttpGet("{memberId}/Comments")]
        public async Task<IActionResult> GetAllCommentsOfUser([FromRoute] string memberId)
        {
            var comments = await _commentRepository.GetAllCommentsOfMember(memberId);
            return Ok(comments);
        }


        [HttpPost("Follow/{communityId}")]
        [Authorize]
        public async Task<IActionResult> FollowCommunity([FromRoute] int communityId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var communityExists = await _communityRepository.CommunityExists(communityId);

            if (!communityExists)
            {
                ModelState.AddModelError("404", "Member or community was not found");
                return BadRequest(ModelState);
            }

            var followed = _followRepository.Follow(curUserId, communityId);
            if (followed)
            {
                var community = await _communityRepository.GetCommunityById(communityId);
                community!.MemberCount += 1;
                _communityRepository.Update(community);
            }

            return Ok();
        }

        [HttpPost("Unfollow/{communityId}")]
        [Authorize]
        public async Task<IActionResult> UnFollowCommunity([FromRoute] int communityId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var communityExists = await _communityRepository.CommunityExists(communityId);

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
                var community = await _communityRepository.GetCommunityById(communityId);
                community!.MemberCount -= 1;
                _communityRepository.Update(community);
            }

            return Ok();
        }

        [HttpPost("{memberId}/Like/{communityId}/{postId}")]
        [Authorize]
        public async Task<IActionResult> LikePost([FromRoute] string memberId, [FromRoute] int postId, [FromRoute] int communityId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var postExists = await _postRepository.PostExists(communityId, postId);

            if (!postExists)
            {
                ModelState.AddModelError("404", "Member of post was not found");
                return BadRequest(ModelState);
            }

            var liked = _likeRepository.LikePost(curUserId, communityId, postId);
            if (liked)
            {
                var post = await _postRepository.GetPostByIdAsync(communityId, postId);
                post!.LikeCount += 1;
            }

            return Ok();
        }


        [HttpPost]
        [Authorize]
        [Route("Like/{communityId}/{postId}/{commentId}")]
        public async Task<IActionResult> LikeComment([FromRoute] int commentId, [FromRoute] int postId, [FromRoute] int communityId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var postExists = await _commentRepository.CommentExists(communityId, postId, commentId);

            if (!postExists)
            {
                ModelState.AddModelError("404", "Member of post was not found");
                return BadRequest(ModelState);
            }

            var liked = _likeRepository.LikeComment(curUserId, communityId, postId, commentId);
            if (liked)
            {
                var post = await _postRepository.GetPostByIdAsync(communityId, postId);
                post!.LikeCount += 1;
            }

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
