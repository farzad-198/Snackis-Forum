using Snackis.Core.Entities;

namespace Snackis.Core.Interfaces
{
    public interface IPrivateMessageService
    {
        Task<List<PrivateMessage>> GetReceivedMessagesAsync(
            string userId);

        Task<List<PrivateMessage>> GetSentMessagesAsync(
            string userId);

        Task SendMessageAsync(
            string senderId,
            string receiverId,
            string? subject,
            string content);
    }
}