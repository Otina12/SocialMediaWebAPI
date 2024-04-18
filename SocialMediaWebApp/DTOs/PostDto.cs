using SocialMediaWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaWebApp.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid CommunityId { get; set; }
        public string MemberId { get; set; }
        public string Content { get; set; }
        public DateTime PostTime { get; set; }
        public DateTime? EditTime { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
