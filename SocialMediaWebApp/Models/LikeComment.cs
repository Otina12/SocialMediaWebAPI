using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaWebApp.Models
{
    [PrimaryKey(nameof(CommentId), nameof(PostId), nameof(CommunityId), nameof(MemberId))]
    public class LikeComment
    {

        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public Guid CommunityId { get; set; }
        public string MemberId { get; set; }
        [ForeignKey("CommentId, PostId, CommunityId")]
        public Comment Comment { get; set; }
        public Member Member { get; set; }
        public DateTime Time { get; set; }
    }
}
