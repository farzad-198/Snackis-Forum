using Snackis.Core.Entities;
using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class ForumService : IForumService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Topic> _topicRepository;
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Comment> _commentRepository;

        public ForumService(
            IRepository<Category> categoryRepository,
            IRepository<Topic> topicRepository,
            IRepository<Post> postRepository,
            IRepository<Comment> commentRepository)
        {
            _categoryRepository = categoryRepository;
            _topicRepository = topicRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }


        // Category methods

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            Category? category =
                await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return false;
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();

            return true;
        }


        // Topic methods

        public async Task<List<Topic>> GetTopicsByCategoryAsync(
            int categoryId)
        {
            List<Topic> topics =
                await _topicRepository.GetAllAsync();

            return topics
                .Where(topic => topic.CategoryId == categoryId)
                .ToList();
        }

        public async Task<Topic?> GetTopicByIdAsync(int id)
        {
            return await _topicRepository.GetByIdAsync(id);
        }

        public async Task AddTopicAsync(Topic topic)
        {
            await _topicRepository.AddAsync(topic);
            await _topicRepository.SaveChangesAsync();
        }

        public async Task UpdateTopicAsync(Topic topic)
        {
            _topicRepository.Update(topic);
            await _topicRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteTopicAsync(int id)
        {
            Topic? topic =
                await _topicRepository.GetByIdAsync(id);

            if (topic == null)
            {
                return false;
            }

            _topicRepository.Delete(topic);
            await _topicRepository.SaveChangesAsync();

            return true;
        }


        // Post methods

        public async Task<List<Post>> GetPostsByTopicAsync(int topicId)
        {
            List<Post> posts =
                await _postRepository.GetAllAsync();

            return posts
                .Where(post => post.TopicId == topicId)
                .ToList();
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _postRepository.GetByIdAsync(id);
        }

        public async Task AddPostAsync(Post post)
        {
            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();
        }


        // Comment methods

        public async Task AddCommentAsync(Comment comment)
        {
            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();
        }
    }
}