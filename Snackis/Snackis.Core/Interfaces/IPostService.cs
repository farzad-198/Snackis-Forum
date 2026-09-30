using Snackis.Core.Entities;

namespace Snackis.Core.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsByTopicAsync(
            int topicId);

        Task<Post?> GetPostByIdAsync(int id);

        Task AddPostAsync(Post post);
    }
}