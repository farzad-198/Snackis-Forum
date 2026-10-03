using Snackis.Core.Entities;
using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;

        public CommentService(
            IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<Comment>> GetCommentsByPostAsync(
            int postId)
        {
            List<Comment> comments =
                await _commentRepository.FindAsync(
                    comment => comment.PostId == postId,
                    comment => comment.User!);

            return comments
                .OrderBy(comment => comment.CreatedAt)
                .ThenBy(comment => comment.Id)
                .ToList();
        }

        public async Task AddCommentAsync(
            Comment comment)
        {
            await _commentRepository.AddAsync(comment);

            await _commentRepository.SaveChangesAsync();
        }
    }
}