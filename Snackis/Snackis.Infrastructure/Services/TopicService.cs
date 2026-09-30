using Snackis.Core.Entities;
using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class TopicService : ITopicService
    {
        private readonly IRepository<Topic>
            _topicRepository;

        public TopicService(
            IRepository<Topic> topicRepository)
        {
            _topicRepository = topicRepository;
        }


        public async Task<List<Topic>>
            GetTopicsByCategoryAsync(int categoryId)
        {
            List<Topic> topics =
                await _topicRepository.GetAllAsync();

            return topics
                .Where(topic =>
                    topic.CategoryId == categoryId)
                .ToList();
        }


        public async Task<Topic?>
            GetTopicByIdAsync(int id)
        {
            return await
                _topicRepository.GetByIdAsync(id);
        }


        public async Task AddTopicAsync(
            Topic topic)
        {
            await _topicRepository.AddAsync(topic);

            await _topicRepository.SaveChangesAsync();
        }


        public async Task UpdateTopicAsync(
            Topic topic)
        {
            _topicRepository.Update(topic);

            await _topicRepository.SaveChangesAsync();
        }


        public async Task<bool> DeleteTopicAsync(
            int id)
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
    }
}