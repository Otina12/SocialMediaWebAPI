using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaWebApp.Models
{
    [PrimaryKey(nameof(CommentId), nameof(MemberId))]
    public class LikeComment
    {
        public Guid CommentId { get; set; }
        public string MemberId { get; set; }
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }
        public DateTime Time { get; set; }
    }
}
