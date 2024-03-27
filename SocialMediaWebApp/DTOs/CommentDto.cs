using SocialMediaWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaWebApp.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int CommunityId { get; set; }
        public string MemberId { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditTime { get; set; }
        public int LikeCount { get; set; }
        public int IsReplyToId { get; set; }
    }
}
