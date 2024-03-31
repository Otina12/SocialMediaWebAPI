using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Extensions;
using SocialMediaWebApp.Interfaces;
using SocialMediaWebApp.Mappers;
using SocialMediaWebApp.Models;
using SocialMediaWebApp.Repositories;
using System.Net.Http;

namespace SocialMediaWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICommunityRepository _communityRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IHttpContextAccessor _httpContext;

        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, 
            ICommunityRepository communityRepository, ILikeRepository likeRepository, IHttpContextAccessor httpContext)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _communityRepository = communityRepository;
            _likeRepository = likeRepository;
            _httpContext = httpContext;
        }


        [HttpGet("{communityId}/{postId}")]
        public async Task<ActionResult<PostDto>> GetPostById([FromRoute] int communityId, [FromRoute] int postId)
        {
            var post = await _postRepository.GetPostByIdAsync(communityId, postId);

            if (post == null)
            {
                return NotFound();
            }

            var postDto = post.MapToPostDto();

            return Ok(postDto);
        }


        [HttpGet("{communityId}/{postId}/Comments")]
        public async Task<ActionResult<List<CommentDto>>> GetAllCommentsOfPost([FromRoute] int communityId, [FromRoute] int postId)
        {
            var comments = await _commentRepository.GetAllCommentsOfPost(communityId, postId);
            var commentDtos = comments.Select(c => c.MapToCommentDto());

            return Ok(commentDtos);
        }


        [HttpPost("{communityId}/Create")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int communityId, [FromBody] CreatePostDto createPostDto)
        {
            var communityExists = await _communityRepository.CommunityExists(communityId);

            if (!communityExists)
            {
                ModelState.AddModelError("404", "Community was not found");
                return BadRequest(ModelState);
            }

            int newPostId = _postRepository.GetFirstAvailableId(communityId);
            

            var post = createPostDto.MapToPost();

            post.CommunityId = communityId;
            post.Id = newPostId;
            post.MemberId = _httpContext.HttpContext!.User.GetCurrentUserId();

            var created = _postRepository.Create(post);

            if (!created)
            {
                ModelState.AddModelError("500", "Something went wrong while adding a comment");
                return BadRequest(ModelState);
            }

            return Ok(post);
        }


        [HttpPatch("{communityId}/{postId}/Edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromRoute] int communityId, [FromRoute] int postId, [FromBody] string content) // we only need content so I'll not use DTO
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var post = await _postRepository.GetPostByIdAsync(communityId, postId);

            if (post == null)
            {
                ModelState.AddModelError("404", "Post was not found");
                return BadRequest(ModelState);
            }

            if (curUserId != post.MemberId)
            {
                return BadRequest("You are not allowed to edit this post");
            }

            post!.Content = content;
            var updated = _postRepository.Update(post);

            if (!updated)
            {
                ModelState.AddModelError("500", "Something went wrong while editing a post");
                return BadRequest(ModelState);
            }

            return Ok(post);
        }

        

        [HttpDelete]
        [Authorize]
        [Route("{communityId}/{postId}/Delete")]
        public async Task<IActionResult> DeletePost([FromRoute] int communityId, [FromRoute] int postId)
        {
            var curUserId = _httpContext.HttpContext!.User.GetCurrentUserId();
            var post = await _postRepository.GetPostByIdAsync(communityId, postId);

            if(post == null)
            {
                ModelState.AddModelError("404", "Post was not found");
                return BadRequest(ModelState);
            }

            if (curUserId != post.MemberId)
            {
                return BadRequest("You are not allowed to delete this post");
            }

            var comments = await _commentRepository.GetAllCommentsOfPost(communityId, postId);
            foreach(var comment in comments)
            {
                _commentRepository.Delete(comment);
            }

            await _likeRepository.RemoveAllLikedOfPost(communityId, postId);

            var deleted = _postRepository.Delete(post);

            if (!deleted)
            {
                ModelState.AddModelError("500", "Something went wrong while deleting a post");
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
