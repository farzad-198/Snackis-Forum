namespace Snackis.Core.Entities
{
    public class Report
    {
        public int Id { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsReviewed { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; } = null!;

        public string ReporterId { get; set; } = string.Empty;

        public AppUser Reporter { get; set; } = null!;
    }
}