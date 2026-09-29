using Snackis.Core.Entities;
using Snackis.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<List<Topic>> GetTopicsByCategoryAsync(int categoryId)
        {
            List<Topic> topics =
                await _topicRepository.GetAllAsync();

            return topics
                .Where(topic => topic.CategoryId == categoryId)
                .ToList();
        }

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

        public async Task AddCommentAsync(Comment comment)
        {
            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();
        }
    }
}

