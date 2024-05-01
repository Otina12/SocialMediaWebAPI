using SocialMedia.Application.Dtos.DtosForGet;
using SocialMedia.Application.Dtos.DtosForPost;
using SocialMedia.Domain.Entites;

namespace SocialMedia.Application.Helpers.Mappers;

public static class PostMapper
{
    public static PostDto MapToPostDto(this Post post)
    {
        return new PostDto
        {
            Id = post.Id,
            MemberId = post.MemberId,
            Content = post.Content,
            PostTime = post.PostTime,
            EditTime = post.EditTime,
            LikeCount = post.LikeCount,
            CommentCount = post.CommentCount
        };
    }

    public static Post MapToPost(this CreatePostDto postDto)
    {
        return new Post
        {
            Id = Guid.Empty,
            Content = postDto.Content,
            PostTime = DateTime.Now,
            IsEdited = false,
            LikeCount = 0,
            CommentCount = 0
        };
    }
}
