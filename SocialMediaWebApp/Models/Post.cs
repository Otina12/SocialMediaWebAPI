using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaWebApp.Models
{
    public class Post
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Member))]
        public string MemberId { get; set; }

        [ForeignKey(nameof(Community))]
        public Guid CommunityId { get; set; }
        public string Content { get; set; }
        public DateTime PostTime { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditTime { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public Member Member { get; set; }
        public Community Community { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<MediaObject>? Media { get; set; } = new List<MediaObject>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
