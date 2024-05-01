using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Persistence.Configurations
{
    public class CommunityConfiguration : IEntityTypeConfiguration<Community>
    {
        public void Configure(EntityTypeBuilder<Community> builder)
        {
            builder.Property(c => c.CreatorId).IsRequired();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Description).HasMaxLength(300);

            builder.HasOne(c => c.Member)
                .WithMany(c => c.CreatedCommunities)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
