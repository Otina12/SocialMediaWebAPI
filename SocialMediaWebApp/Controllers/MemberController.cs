using Microsoft.AspNetCore.Authorization;
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
        private readonly IHttpContextAccessor _httpContext;

        public MemberController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
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

            var followed = _unitOfWork.Followings.Follow(curUserId, communityId);
            if (followed)
            {
                var community = await _unitOfWork.Communities.GetByIdAsync(communityId);
                community!.MemberCount += 1;
                await _unitOfWork.Communities.Update(community);
            }

            await _unitOfWork.SaveChangesAsync();

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

            var following = await _unitOfWork.Followings.GetFollowingById(curUserId, communityId);
            if (following is null)
            {
                ModelState.AddModelError("404", "Member was not followed to a community");
                return BadRequest(ModelState);
            }

            var followed = _unitOfWork.Followings.Unfollow(following!);
            if (followed)
            {
                var community = await _unitOfWork.Communities.GetByIdAsync(communityId);
                community!.MemberCount -= 1;
                await _unitOfWork.Communities.Update(community);
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Like/{postId}")]
        [Authorize]
        public async Task<IActionResult> LikePost([FromRoute] Guid postId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var postExists = await _unitOfWork.Posts.PostExists(postId);

            if (!postExists)
            {
                ModelState.AddModelError("404", "Member of post was not found");
                return BadRequest(ModelState);
            }

            var liked = _unitOfWork.Likes.LikePost(curUserId, postId);
            if (liked)
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(postId);
                post!.LikeCount += 1;
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }


        [HttpPost]
        [Authorize]
        [Route("LikeComment/{commentId}")]
        public async Task<IActionResult> LikeComment([FromRoute] Guid commentId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var postExists = await _unitOfWork.Comments.CommentExists(commentId);

            if (!postExists)
            {
                ModelState.AddModelError("404", "Member of post was not found");
                return BadRequest(ModelState);
            }

            _unitOfWork.Likes.LikeComment(curUserId, commentId);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("Followings")]
        public async Task<ActionResult<List<CommunityDto>>> GetAllFollowingsOfMember()
        {
            var memberId = _httpContext.HttpContext?.User.GetCurrentUserId();
            var communities = await _unitOfWork.Communities.GetAllFollowingsOfMember(memberId!);

            var communitiesDto = communities.Select(c => c.MapToCommunityDto());

            return Ok(communitiesDto);
        }

        [HttpPatch("Edit")]
        [Authorize]
        public async Task<IActionResult> EditProfile([FromBody] MemberEditDto memberDto)
        {
            var curMemberId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var curMember = await _unitOfWork.Members.GetMemberById(curMemberId);

            curMember!.UserName = memberDto.UserName is null ? curMember.UserName : memberDto.UserName;
            curMember!.Bio = memberDto.Bio is null ? curMember.Bio : memberDto.Bio;
            curMember!.ProfilePhotoUrl = memberDto.ProfilePhotoUrl is null ? curMember.ProfilePhotoUrl : memberDto.ProfilePhotoUrl;

            _unitOfWork.Members.Update(curMember);
            return Ok(curMember.MapToFollowerDto());
        }
    }
}
