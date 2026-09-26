namespace Snackis.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int PostId { get; set; }

        public Post Post { get; set; } = null!;

        public string AuthorName { get; set; } = string.Empty;

        public string? UserId { get; set; }

        public AppUser? User { get; set; }
    }
}