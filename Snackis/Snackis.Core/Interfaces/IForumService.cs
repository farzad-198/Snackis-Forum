using Snackis.Core.Entities;

namespace Snackis.Core.Interfaces
{
    public interface IForumService
    {
        // Category methods
        Task<List<Category>> GetCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int id);

        Task AddCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category category);

        Task<bool> DeleteCategoryAsync(int id);


        // Topic methods
        Task<List<Topic>> GetTopicsByCategoryAsync(int categoryId);

        Task<Topic?> GetTopicByIdAsync(int id);

        Task AddTopicAsync(Topic topic);

        Task UpdateTopicAsync(Topic topic);

        Task<bool> DeleteTopicAsync(int id);


        // Post methods
        Task<List<Post>> GetPostsByTopicAsync(int topicId);

        Task<Post?> GetPostByIdAsync(int id);

        Task AddPostAsync(Post post);


        // Comment methods
        Task AddCommentAsync(Comment comment);
    }
}