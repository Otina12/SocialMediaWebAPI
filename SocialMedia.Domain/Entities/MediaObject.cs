using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Domain.Entites
{
    public class MediaObject
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public Guid PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
