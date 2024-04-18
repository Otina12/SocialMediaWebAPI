using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;
using SocialMediaWebApp.Repositories;

namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IHttpContextAccessor _httpContext;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository, IHttpContextAccessor httpContext)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _httpContext = httpContext;
        }

        [HttpGet("{communityId}/{postId}/{commentId}")]
        public async Task<ActionResult<List<CommentDto>>> GetCommentOfPost([FromRoute] Guid communityId, [FromRoute] Guid postId, [FromRoute] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentById(communityId, postId, commentId);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDtos = comment.MapToCommentDto();

            return Ok(commentDtos);
        }


        [HttpGet("{communityId}/{postId}/{commentId}/Replies")]
        public async Task<ActionResult<List<CommentDto>>> GetRepliesOfComment([FromRoute] Guid communityId, [FromRoute] Guid postId, [FromRoute] Guid commentId)
        {
            var comments = await _commentRepository.GetAllRepliesOfAComment(communityId, postId, commentId);
            var commentDtos = comments.Select(c => c.MapToCommentDto());

            return Ok(commentDtos);
        }


        [HttpPost("{communityId}/{postId}/Write")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromRoute] Guid communityId, [FromRoute] Guid postId, [FromBody] CreateCommentDto createCommentDto)
        {
            var postExists = await _postRepository.PostExists(communityId, postId);

            if (!postExists)
            {
                ModelState.AddModelError("404", "Post or community was not found");
                return BadRequest(ModelState);
            }

            var comment = createCommentDto.MapToComment();

            comment.Id = Guid.NewGuid();
            comment.PostId = postId;
            comment.CommunityId = communityId;
            comment.MemberId = _httpContext.HttpContext!.User.GetCurrentUserId();

            var created = _commentRepository.Create(comment);

            if (!created)
            {
                ModelState.AddModelError("500", "Something went wrong while adding a comment");
                return BadRequest(ModelState);
            }

            return Ok(comment);
        }


        [HttpPatch("{communityId}/{postId}/{commentId}/Edit")]
        [Authorize]
        public async Task<IActionResult> EditComment([FromRoute] Guid communityId, [FromRoute] Guid postId, [FromRoute] Guid commentId, [FromBody] string content)
        {
            var comment = await _commentRepository.GetCommentById(communityId, postId, commentId);

            if (comment == null)
            {
                ModelState.AddModelError("404", "Comment was not found");
                return BadRequest(ModelState);
            }

            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            if (curUserId != comment.MemberId)
            {
                return BadRequest("You are not allowed to edit this comment");
            }

            comment!.Content = content;
            var updated = _commentRepository.Update(comment);

            if (!updated)
            {
                ModelState.AddModelError("500", "Something went wrong while editing a comment");
                return BadRequest(ModelState);
            }

            return Ok(comment);
        }

        [HttpPost("{communityId}/{postId}/{commentId}/Replies/Add")]
        [Authorize]
        public async Task<IActionResult> AddReply([FromRoute] Guid communityId, [FromRoute] Guid postId,
            [FromRoute] Guid commentId, [FromBody] CreateCommentDto createCommentDto)
        {
            var commentExists = await _commentRepository.CommentExists(communityId, postId, commentId);

            if (!commentExists)
            {
                ModelState.AddModelError("404", "Comment was not found");
                return BadRequest(ModelState);
            }

            var comment = createCommentDto.MapToComment();

            comment.Id = Guid.NewGuid();
            comment.PostId = postId;
            comment.CommunityId = communityId;
            comment.IsReply = true;
            comment.IsReplyToId = commentId;
            comment.MemberId = _httpContext.HttpContext!.User.GetCurrentUserId();

            var created = _commentRepository.Create(comment);

            if (!created)
            {
                ModelState.AddModelError("500", "Something went wrong while adding a reply");
                return BadRequest(ModelState);
            }

            return Ok(comment);
        }


        [HttpDelete]
        [Authorize]
        [Route("Delete/{communityId}/{postId}/{commentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid communityId, [FromRoute] Guid postId, [FromRoute] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentById(communityId, postId, commentId);

            if(comment is null)
            {
                ModelState.AddModelError("404", "Community was not found");
                return BadRequest(ModelState);
            }

            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            if (curUserId != comment.MemberId)
            {
                return BadRequest("You are not allowed to delete this comment");
            }

            var replies = await _commentRepository.GetAllRepliesOfAComment(communityId, postId, commentId);
            foreach(var reply in replies)
            {
                _commentRepository.Delete(reply);
            }

            _commentRepository.Delete(comment);

            return Ok();
        }
    }
}
