using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.Core.IConfiguration;
using SocialMediaWebApp.Core.IRepositories;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public CommentController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<List<CommentDto>>> GetCommentOfPost([FromRoute] Guid commentId)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(commentId);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDtos = comment.MapToCommentDto();

            return Ok(commentDtos);
        }


        [HttpGet("{commentId}/Replies")]
        public async Task<ActionResult<List<CommentDto>>> GetRepliesOfComment(Guid commentId)
        {
            var comments = await _unitOfWork.Comments.GetAllRepliesOfAComment(commentId);
            var commentDtos = comments.Select(c => c.MapToCommentDto());

            return Ok(commentDtos);
        }


        [HttpPost("/{postId}/AddComment")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromRoute] Guid postId, [FromBody] CreateCommentDto createCommentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Enter valid input");
            }

            var postExists = await _unitOfWork.Posts.PostExists(postId);

            if (!postExists)
            {
                ModelState.AddModelError("404", "Post or community was not found");
                return BadRequest(ModelState);
            }

            var comment = createCommentDto.MapToComment();

            comment.Id = Guid.NewGuid();
            comment.MemberId = _httpContext.HttpContext!.User.GetCurrentUserId();
            comment.PostId = postId;
            comment.IsReplyToId = null;

            var created = await _unitOfWork.Comments.Add(comment);

            if (!created)
            {
                ModelState.AddModelError("500", "Something went wrong while adding a comment");
                return BadRequest(ModelState);
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(comment);
        }


        [HttpPatch("{commentId}/Edit")]
        [Authorize]
        public async Task<IActionResult> EditComment([FromRoute] Guid commentId, [FromBody] CreateCommentDto createCommentDto)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(commentId);

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

            comment!.Content = createCommentDto.Content;
            comment.IsEdited = true;
            comment.EditTime = DateTime.Now;

            var updated = await _unitOfWork.Comments.Update(comment);

            if (!updated)
            {
                ModelState.AddModelError("500", "Something went wrong while editing a comment");
                return BadRequest(ModelState);
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpPost("{commentId}/Replies/Add")]
        [Authorize]
        public async Task<IActionResult> AddReply([FromRoute] Guid commentId, [FromBody] CreateCommentDto createCommentDto)
        {
            var existingComment = await _unitOfWork.Comments.GetByIdAsync(commentId);

            if (existingComment is null)
            {
                ModelState.AddModelError("404", "Comment was not found");
                return BadRequest(ModelState);
            }

            var comment = createCommentDto.MapToComment();

            comment.Id = Guid.NewGuid();
            comment.MemberId = _httpContext.HttpContext!.User.GetCurrentUserId();
            comment.PostId = existingComment.PostId;
            comment.IsReply = true;
            comment.IsReplyToId = commentId;
            comment.EditTime = DateTime.Now;
            

            var created = await _unitOfWork.Comments.Add(comment);

            if (!created)
            {
                ModelState.AddModelError("500", "Something went wrong while adding a reply");
                return BadRequest(ModelState);
            }

            await _unitOfWork.SaveChangesAsync();

            var commentDto = comment.MapToCommentDto();
            return Ok(commentDto);
        }


        [HttpDelete]
        [Authorize]
        [Route("{commentId}/Delete")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid commentId)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(commentId);

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

            var replies = await _unitOfWork.Comments.GetAllRepliesOfAComment(commentId);

            foreach(var reply in replies)
            {
                await _unitOfWork.Comments.Delete(reply.Id);
            }

            await _unitOfWork.Comments.Delete(commentId);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
