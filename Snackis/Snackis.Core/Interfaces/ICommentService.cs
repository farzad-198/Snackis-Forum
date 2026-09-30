using Snackis.Core.Entities;

namespace Snackis.Core.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(Comment comment);
    }
}