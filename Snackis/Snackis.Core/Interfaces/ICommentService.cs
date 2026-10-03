using Snackis.Core.Entities;

namespace Snackis.Core.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetCommentsByPostAsync(int postId);
        Task AddCommentAsync(Comment comment);
    }
}