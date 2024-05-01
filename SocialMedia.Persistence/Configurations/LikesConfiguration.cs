using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Domain.Entites;


namespace SocialMedia.Persistence.Configurations
{
    public class LikesConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasOne(c => c.Member)
                .WithMany(c => c.LikedPosts)
                .HasForeignKey(c => c.MemberId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
