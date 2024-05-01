using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Domain.Entites
{
    public class Member : IdentityUser
    {
        public string? Bio { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public int FollowerCount { get; set; }
        public DateTime JoinDate { get; set; }
        public ICollection<Like> LikedPosts { get; set; } = new List<Like>();
        public ICollection<LikeComment> LikedComments { get; set; } = new List<LikeComment>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Following> FollowedCommunities { get; set; } = new List<Following>();
        public ICollection<Community> CreatedCommunities { get; set; } = new List<Community>();
        public ICollection<Post> PublishedPosts { get; set; } = new List<Post>();

        public object MapToFollowerDto()
        {
            throw new NotImplementedException();
        }
    }
}
