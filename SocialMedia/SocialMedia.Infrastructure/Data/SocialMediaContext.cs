namespace SocialMedia.Infrastructure.Data;

using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Entities;
using SocialMedia.Infrastructure.Data;


public class SocialMediaContext : DbContext
{
    public SocialMediaContext(DbContextOptions<SocialMediaContext> options) : base(options){}

    public DbSet<User> Users { get; set;}
    public DbSet<Post> Posts { get; set;}
    public DbSet<Like> Likes { get; set;}
    public DbSet<Comment> Comments { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
            entity.HasIndex(u => u.Username).IsUnique();

            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.Bio).HasMaxLength(200);

            entity.Property(u => u.CreatedAt).HasColumnType("date").HasDefaultValueSql("SYSDATETIME()");

           entity.Property(u => u.PasswordHash) 
            .IsRequired() 
            .HasMaxLength(255) 
            .HasColumnName("PasswordHash"); 

            entity.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);

            entity.HasKey(u => u.UserId);
        });

        //Post configuration
        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(p => p.Content).IsRequired().HasMaxLength(500);
            entity.Property(p => p.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("SYSDATETIME()");
            entity.Property(p => p.LikesCount).IsRequired().HasDefaultValue(0);

            entity.HasKey(p => p.PostId);
        });
   
        //Comment configuration
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(c => c.Content).IsRequired().HasMaxLength(300);
            entity.Property(c => c.CreatedAt).HasColumnType("datetime").HasDefaultValueSql("SYSDATETIME()");

            entity.HasKey(c => c.Id);
        });

        //Relationships (User-Post)
        modelBuilder.Entity<Post>()
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //Relationships (User-Like | Like-)
        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade); 
        });

        //Relationships (User-Comment | Comment-Post)
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}