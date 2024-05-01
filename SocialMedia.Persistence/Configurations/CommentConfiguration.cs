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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(c => c.Content).IsRequired().HasMaxLength(200);
            builder.Property(c => c.MemberId).IsRequired();

            builder.HasOne(c => c.Member)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MemberId);

            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Reply)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.IsReplyToId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Replies)
                .WithOne(r => r.Reply)
                .HasForeignKey(c => c.IsReplyToId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
