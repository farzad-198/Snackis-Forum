using Snackis.Core.Entities;
using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class PrivateMessageService : IPrivateMessageService
    {
        private readonly IRepository<PrivateMessage> _messageRepository;
        private readonly IRepository<AppUser> _userRepository;

        public PrivateMessageService(
            IRepository<PrivateMessage> messageRepository,
            IRepository<AppUser> userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task<List<PrivateMessage>> GetReceivedMessagesAsync(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "The user ID is required.");
            }

            List<PrivateMessage> messages =
                await _messageRepository.FindAsync(
                    message => message.ReceiverId == userId,
                    message => message.Sender);

            return messages
                .OrderByDescending(message => message.SentAt)
                .ThenByDescending(message => message.Id)
                .ToList();
        }

        public async Task<List<PrivateMessage>> GetSentMessagesAsync(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(
                    "The user ID is required.");
            }

            List<PrivateMessage> messages =
                await _messageRepository.FindAsync(
                    message => message.SenderId == userId,
                    message => message.Receiver);

            return messages
                .OrderByDescending(message => message.SentAt)
                .ThenByDescending(message => message.Id)
                .ToList();
        }

        public async Task SendMessageAsync(
            string senderId,
            string receiverId,
            string? subject,
            string content)
        {
            if (string.IsNullOrWhiteSpace(senderId)
                || string.IsNullOrWhiteSpace(receiverId))
            {
                throw new ArgumentException(
                    "The sender and receiver are required.");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException(
                    "Please enter a message.");
            }

            content = content.Trim();

            if (content.Length > 5000)
            {
                throw new ArgumentException(
                    "The message cannot exceed 5000 characters.");
            }

            subject = string.IsNullOrWhiteSpace(subject)
                ? null
                : subject.Trim();

            if (subject != null && subject.Length > 150)
            {
                throw new ArgumentException(
                    "The subject cannot exceed 150 characters.");
            }

            List<AppUser> users =
                await _userRepository.FindAsync(
                    user => user.Id == senderId
                        || user.Id == receiverId);

            if (!users.Any(user => user.Id == senderId))
            {
                throw new ArgumentException(
                    "The sender account was not found.");
            }

            if (!users.Any(user => user.Id == receiverId))
            {
                throw new ArgumentException(
                    "The receiver account was not found.");
            }

            PrivateMessage message = new()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Subject = subject,
                Content = content,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _messageRepository.AddAsync(message);

            await _messageRepository.SaveChangesAsync();
        }
    }
}