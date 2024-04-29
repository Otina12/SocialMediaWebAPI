using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SocialMediaWebApp.Core.IConfiguration;
using SocialMediaWebApp.Core.IRepositories;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Features.Posts.Commands;
using SocialMediaWebApp.Features.Posts.Queries;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;
using System.Net.Http;

namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : BaseController
    {
        private readonly IMediator _mediator;

        public PostController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext, IMediator mediator) : base(unitOfWork, httpContext)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _mediator.Send(new GetAllPostsQuery());

            return Ok(result);
        }


        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostById([FromRoute] Guid postId)
        {
            var result = await _mediator.Send(new GetPostByIdQuery(postId));

            return result is null ? NotFound() : Ok(result);
        }


        [HttpPost("{communityId}/Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] Guid communityId, [FromBody] CreatePostDto createPostDto, IValidator<CreatePostCommand> val)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var command = new CreatePostCommand(communityId, curUserId, createPostDto);

            var validationResult = await val.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToDictionary(error => (error.ErrorCode), error => error.ErrorMessage));
            }

            var result = await _mediator.Send(command);

            return await GetPostById(result.Id);
        }


        [HttpPatch("{postId}/Edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromRoute] Guid postId, [FromBody] CreatePostDto postDto, IValidator<EditPostCommand> val)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var command = new EditPostCommand(postId, curUserId, postDto);

            var validationResult = await val.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.ToDictionary(error => (error.ErrorCode), error => error.ErrorMessage));
            }

            var result = await _mediator.Send(command);

            return Ok(result);
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
