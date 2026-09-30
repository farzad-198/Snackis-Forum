using Snackis.Core.Entities;
using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment>
            _commentRepository;

        public CommentService(
            IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }


        public async Task AddCommentAsync(
            Comment comment)
        {
            await _commentRepository.AddAsync(comment);

            await _commentRepository.SaveChangesAsync();
        }
    }
}