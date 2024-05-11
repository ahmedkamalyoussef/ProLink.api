using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IMessageService
    {
        Task<List<MessageResultDto>> GetMessagesAsync(string recieverId);
        Task<bool> SendMessageAsync(string senderId, SendMessageDto sendMessageDto);
        Task<bool> UpdateMessageAsync(string messageId, SendMessageDto sendMessageDto);
        Task<bool> DeleteMessageAsync(string messageId);
    }
}
