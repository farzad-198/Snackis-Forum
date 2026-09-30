using Snackis.Core.Entities;
using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post>
            _postRepository;

        public PostService(
            IRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }


        public async Task<List<Post>>
            GetPostsByTopicAsync(int topicId)
        {
            List<Post> posts =
                await _postRepository.GetAllAsync();

            return posts
                .Where(post =>
                    post.TopicId == topicId)
                .ToList();
        }


        public async Task<Post?>
            GetPostByIdAsync(int id)
        {
            return await
                _postRepository.GetByIdAsync(id);
        }


        public async Task AddPostAsync(Post post)
        {
            await _postRepository.AddAsync(post);

            await _postRepository.SaveChangesAsync();
        }
    }
}