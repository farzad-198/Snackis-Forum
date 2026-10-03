using Snackis.Core.Entities;
using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRepository;

        public PostService(IRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<Post>> GetPostsByTopicAsync(
            int topicId)
        {
            return await _postRepository.FindAsync(
                post => post.TopicId == topicId,
                post => post.User);
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            List<Post> posts =
                await _postRepository.FindAsync(
                    post => post.Id == id,
                    post => post.User);

            return posts.FirstOrDefault();
        }

        public async Task AddPostAsync(Post post)
        {
            await _postRepository.AddAsync(post);

            await _postRepository.SaveChangesAsync();
        }
    }
}