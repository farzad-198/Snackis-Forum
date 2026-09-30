using Snackis.Core.Entities;

namespace Snackis.Core.Interfaces
{
    public interface ITopicService
    {
        Task<List<Topic>> GetTopicsByCategoryAsync(
            int categoryId);

        Task<Topic?> GetTopicByIdAsync(int id);

        Task AddTopicAsync(Topic topic);

        Task UpdateTopicAsync(Topic topic);

        Task<bool> DeleteTopicAsync(int id);
    }
}