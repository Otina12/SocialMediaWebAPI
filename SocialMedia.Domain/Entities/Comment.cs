using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Domain.Entites
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; } // fk to post table
        public required string MemberId { get; set; } // fk to member table
        public Guid? IsReplyToId { get; set; }
        public bool IsReply { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreateTime { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditTime { get; set; }
        public int LikeCount { get; set; }

        [ForeignKey("IsReplyToId")]
        public virtual Comment? Reply { get; set; }
       
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
        public ICollection<LikeComment> Likes { get; set; } = new List<LikeComment>();
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();

        public object MapToCommentDto()
        {
            throw new NotImplementedException();
        }
    }
}