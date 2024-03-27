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
                PostId = Comment.PostId,
                CommunityId = Comment.CommunityId,
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
                Id = 0,
                PostId = 0,
                CommunityId = 0,
                Content = commentDto.Content,
                CreateTime = DateTime.Now,
                IsEdited = false,
                LikeCount = 0,
                IsReplyToId = 0
            };
        }
    }
}
