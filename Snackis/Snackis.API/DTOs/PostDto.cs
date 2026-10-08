namespace Snackis.API.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}