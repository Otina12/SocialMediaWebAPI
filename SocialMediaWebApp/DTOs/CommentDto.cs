using SocialMediaWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaWebApp.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid CommunityId { get; set; }
        public string MemberId { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditTime { get; set; }
        public int LikeCount { get; set; }
        public Guid IsReplyToId { get; set; }
    }
}
