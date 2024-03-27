using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaWebApp.Models
{
    public class MediaObject
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public int PostId { get; set; }
        public int CommunityId { get; set; }
        [ForeignKey("PostId, CommunityId")]
        public Post Post { get; set; }
    }
}
