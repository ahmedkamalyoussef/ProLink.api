using Microsoft.AspNetCore.SignalR;
using ProLink.Application.Interfaces;
using ProLink.Application.DTOs;

namespace ProLink.Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(string receiverId, SendMessageDto sendMessageDto)
        {
            var messageResult = await _messageService.SendMessageAsync(receiverId, sendMessageDto);
            if (messageResult)
            {
                var senderId = Context.UserIdentifier;
                var messages = await _messageService.GetMessagesAsync(receiverId);
                await Clients.User(receiverId).SendAsync("ReceiveMessage", messages);
                await Clients.User(senderId).SendAsync("ReceiveMessage", messages);
            }
        }

        public async Task UpdateMessage(string messageId, SendMessageDto sendMessageDto)
        {
            var updateResult = await _messageService.UpdateMessageAsync(messageId, sendMessageDto);
            if (updateResult)
            {
                var message = await _messageService.GetMessageByIdAsync(messageId);
                await Clients.All.SendAsync("UpdateMessage", message);
            }
        }

        public async Task DeleteMessage(string messageId)
        {
            var deleteResult = await _messageService.DeleteMessageAsync(messageId);
            if (deleteResult)
            {
                await Clients.All.SendAsync("DeleteMessage", messageId);
            }
        }
    }
}
