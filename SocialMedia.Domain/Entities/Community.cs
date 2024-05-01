using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Domain.Entites
{
    public class Community
    {
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public string? PfpUrl { get; set; }
        public int MemberCount { get; set; }

        [ForeignKey(nameof(Member))]
        public string CreatorId { get; set; }
        public Member Member { get; set; }
        public ICollection<Following> Followers { get; set; } = new List<Following>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public object MapToCommunityDto()
        {
            throw new NotImplementedException();
        }
    }
}
