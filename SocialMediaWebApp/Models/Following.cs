using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaWebApp.Models
{
    [PrimaryKey(nameof(FollowerId), nameof(CommunityId))]
    public class Following
    {
        [ForeignKey(nameof(Follower))]
        public string FollowerId { get; set; }
        [ForeignKey(nameof(Community))]
        public Guid CommunityId { get; set; }
        public Member Follower { get; set; }
        public Community Community { get; set; }
    }
}
