using Microsoft.AspNetCore.Identity;

namespace Snackis.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;

        public string? ProfilePicturePath { get; set; }

        public ICollection<Post> Posts { get; set; }
            = new List<Post>();

        public ICollection<Comment> Comments { get; set; }
            = new List<Comment>();

        public ICollection<PrivateMessage> SentMessages { get; set; }
            = new List<PrivateMessage>();

        public ICollection<PrivateMessage> ReceivedMessages { get; set; }
            = new List<PrivateMessage>();

        public ICollection<Report> Reports { get; set; }
            = new List<Report>();
    }
}