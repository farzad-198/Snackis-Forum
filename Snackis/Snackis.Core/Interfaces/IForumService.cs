using Snackis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snackis.Core.Interfaces
{
    public interface IForumService
    {
        Task<List<Category>> GetCategoriesAsync();

        Task<List<Topic>> GetTopicsByCategoryAsync(int categoryId);

        Task<List<Post>> GetPostsByTopicAsync(int topicId);

        Task<Post?> GetPostByIdAsync(int id);

        Task AddPostAsync(Post post);

        Task AddCommentAsync(Comment comment);
    }
}
