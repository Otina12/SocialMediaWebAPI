using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SocialMediaWebApp.Core.IConfiguration;
using SocialMediaWebApp.Core.IRepositories;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;
using System.Net.Http;

namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public PostController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }


        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid postId)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(postId);

            if (post == null)
            {
                Log.Error("Post [PostId: {postId}] was not found", postId);
                return NotFound();
            }

            var postDto = post.MapToPostDto();
            Log.Information("Returned post {@post}", post);

            return Ok(postDto);
        }


        [HttpGet("{postId}/Comments")]
        public async Task<ActionResult<List<CommentDto>>> GetAllCommentsOfPost([FromRoute] Guid postId)
        {
            var comments = await _unitOfWork.Comments.GetAllCommentsOfPost(postId);
            var commentDtos = comments.Select(c => c.MapToCommentDto());

            return Ok(commentDtos);
        }


        [HttpPost("{communityId}/Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] Guid communityId, [FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Enter valid input");
            }

            var communityExists = await _unitOfWork.Communities.CommunityExists(communityId);

            if (!communityExists)
            {
                ModelState.AddModelError("404", "Community was not found");
                return BadRequest(ModelState);
            }

            var post = createPostDto.MapToPost();

            post.Id = Guid.NewGuid();
            post.MemberId = _httpContext.HttpContext!.User.GetCurrentUserId();
            post.CommunityId = communityId;

            var created = await _unitOfWork.Posts.Add(post);

            if (!created)
            {
                ModelState.AddModelError("500", "Something went wrong while adding a comment");
                return BadRequest(ModelState);
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok(post);
        }


        [HttpPatch("{postId}/Edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromRoute] Guid postId, [FromBody] CreatePostDto postDto)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var post = await _unitOfWork.Posts.GetByIdAsync(postId);

            if (post is null)
            {
                ModelState.AddModelError("404", "Post was not found");
                return BadRequest(ModelState);
            }

            if (curUserId != post.MemberId)
            {
                return BadRequest("You are not allowed to edit this post");
            }

            post!.Content = postDto.Content;
            var updated = await _unitOfWork.Posts.Update(post);

            if (!updated)
            {
                ModelState.AddModelError("500", "Something went wrong while editing a post");
                return BadRequest(ModelState);
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok(post);
        }


        [HttpDelete]
        [Authorize]
        [Route("{postId}/Delete")]
        public async Task<IActionResult> DeletePost([FromRoute] Guid postId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var post = await _unitOfWork.Posts.GetByIdAsync(postId);

            if(post is null)
            {
                ModelState.AddModelError("404", "Post was not found");
                return BadRequest(ModelState);
            }

            if (curUserId != post.MemberId)
            {
                return BadRequest("You are not allowed to delete this post");
            }

            var comments = await _unitOfWork.Comments.GetAllCommentsOfPost(postId);
            foreach(var comment in comments)
            {
                await _unitOfWork.Comments.Delete(comment.Id);
            }

            await _unitOfWork.Likes.RemoveAllLikedOfPost(postId);

            var deleted = await _unitOfWork.Posts.Delete(postId);

            if (!deleted)
            {
                ModelState.AddModelError("500", "Something went wrong while deleting a post");
                return BadRequest(ModelState);
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
