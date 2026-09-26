using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snackis.Core.Entities;

namespace Snackis.Infrastructure.Data
{
    public class SnackisDbContext : IdentityDbContext<AppUser>
    {
        public SnackisDbContext(
            DbContextOptions<SnackisDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Topic> Topics { get; set; } = null!;

        public DbSet<Post> Posts { get; set; } = null!;

        public DbSet<Comment> Comments { get; set; } = null!;

        public DbSet<PrivateMessage> PrivateMessages { get; set; } = null!;

        public DbSet<Report> Reports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Topic>()
                .HasOne(topic => topic.Category)
                .WithMany(category => category.Topics)
                .HasForeignKey(topic => topic.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(post => post.Topic)
                .WithMany(topic => topic.Posts)
                .HasForeignKey(post => post.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(post => post.User)
                .WithMany(user => user.Posts)
                .HasForeignKey(post => post.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.Post)
                .WithMany(post => post.Comments)
                .HasForeignKey(comment => comment.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(comment => comment.User)
                .WithMany(user => user.Comments)
                .HasForeignKey(comment => comment.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PrivateMessage>()
                .HasOne(message => message.Sender)
                .WithMany(user => user.SentMessages)
                .HasForeignKey(message => message.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrivateMessage>()
                .HasOne(message => message.Receiver)
                .WithMany(user => user.ReceivedMessages)
                .HasForeignKey(message => message.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(report => report.Post)
                .WithMany(post => post.Reports)
                .HasForeignKey(report => report.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Report>()
                .HasOne(report => report.Reporter)
                .WithMany(user => user.Reports)
                .HasForeignKey(report => report.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}