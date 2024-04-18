using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaWebApp.Models
{
    public class MediaObject
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public Guid PostId { get; set; }
        public Guid CommunityId { get; set; }
        [ForeignKey("PostId, CommunityId")]
        public Post Post { get; set; }
    }
}
