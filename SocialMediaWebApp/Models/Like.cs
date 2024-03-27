using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaWebApp.Models
{
    [PrimaryKey(nameof(PostId), nameof(CommunityId), nameof(MemberId))]
    public class Like
    {
        public int PostId { get; set; }
        public int CommunityId { get; set; }
        public string MemberId { get; set; }
        [ForeignKey("PostId, CommunityId")]
        public Post Post { get; set; }
        public Member Member { get; set; }
        public DateTime Time { get; set; }
    }
}
