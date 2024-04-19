using SocialMediaWebApp.DTOs;
using SocialMediaWebApp.Models;

namespace SocialMediaWebApp.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto MapToCommentDto(this Comment Comment)
        {
            return new CommentDto
            {
                Id = Comment.Id,
                MemberId = Comment.MemberId,
                Content = Comment.Content,
                CreateTime = Comment.CreateTime,
                EditTime = Comment.EditTime,
                LikeCount = Comment.LikeCount,
                IsReplyToId = Comment.IsReplyToId
            };
        }

        public static Comment MapToComment(this CreateCommentDto commentDto)
        {
            return new Comment
            {
                Id = Guid.Empty,
                MemberId = string.Empty,
                Content = commentDto.Content,
                CreateTime = DateTime.Now,
                IsEdited = false,
                LikeCount = 0,
                IsReplyToId = Guid.Empty
            };
        }
    }
}
