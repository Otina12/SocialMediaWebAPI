using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Entites;

namespace SocialMedia.Persistence;

public class ApplicationDbContext : IdentityDbContext<Member>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            var entities = ChangeTracker
                .Entries()
                .Where(e => e.Entity != null && (e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted))
                .ToList();


            foreach (var entity in entities)
            {
                entity.State = entity.State switch
                {
                    EntityState.Added => EntityState.Modified,
                    _ => EntityState.Unchanged,
                };
            }
            throw;
        }
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Community> Communities { get; set; }
    public DbSet<MediaObject> MediaObjects { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Following> Followings { get; set; }
    public DbSet<LikeComment> LikeComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
        

        List<IdentityRole> roles = new List<IdentityRole>() {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

        modelBuilder.Entity<IdentityRole>().HasData(roles);

        modelBuilder.Entity<Member>().ToTable("Members");
        modelBuilder.Entity<Post>().ToTable("Posts");
        modelBuilder.Entity<Community>().ToTable("Communities");
        modelBuilder.Entity<MediaObject>().ToTable("MediaObjects");
        modelBuilder.Entity<Comment>().ToTable("Comments");


        modelBuilder.Entity<Like>()
            .HasOne(l => l.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(l => l.PostId);

        modelBuilder.Entity<Like>()
            .HasOne(l => l.Member)
            .WithMany(m => m.LikedPosts)
            .HasForeignKey(l => l.MemberId);

        modelBuilder.Entity<LikeComment>()
            .HasOne(l => l.Comment)
            .WithMany(c => c.Likes)
            .HasForeignKey(l => l.CommentId);

        modelBuilder.Entity<LikeComment>()
            .HasOne(l => l.Member)
            .WithMany(c => c.LikedComments)
            .HasForeignKey(l => l.MemberId);

        modelBuilder.Entity<Following>()
            .HasOne(f => f.Follower)
            .WithMany(f => f.FollowedCommunities)
            .HasForeignKey(f => f.FollowerId);

        modelBuilder.Entity<Following>()
            .HasOne(f => f.Community)
            .WithMany(c => c.Followers)
            .HasForeignKey(f => f.CommunityId);
    }
}
