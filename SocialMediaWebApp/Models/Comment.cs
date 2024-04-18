using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaWebApp.Models
{
    [PrimaryKey(nameof(Id), nameof(PostId), nameof(CommunityId))]
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid CommunityId { get; set; }
        public string MemberId { get; set; }
        public bool IsReply { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditTime { get; set; }
        public int LikeCount { get; set; }
        public Guid IsReplyToId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

        [ForeignKey("PostId, CommunityId")]
        public virtual Post Post { get; set; }
        public ICollection<LikeComment> Likes { get; set; } = new List<LikeComment>();
    }
}