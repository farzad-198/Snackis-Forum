namespace Snackis.Core.Entities
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int TopicId { get; set; }

        public Topic Topic { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;

        public AppUser User { get; set; } = null!;

        public ICollection<Comment> Comments { get; set; }
            = new List<Comment>();

        public ICollection<Report> Reports { get; set; }
            = new List<Report>();
    }
}