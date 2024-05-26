
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Data.Consts;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class MessageService: IMessageService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;

        #endregion

        #region ctor
        public MessageService(IUnitOfWork unitOfWork,
            UserManager<User> userManager, IMapper mapper,
            IUserHelpers userHelpers
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _userHelpers = userHelpers;
        }
        #endregion

        #region Message handling
        public async Task<List<MessageResultDto>> GetMessagesAsync(string recieverId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("current user not found");
            var reciever = await _userManager.FindByIdAsync(recieverId);
            if (reciever == null) throw new Exception("reciever not found");
            var messages=await _unitOfWork.Message.FindAsync(m=>(m.ReceiverId == recieverId&&m.SenderId==currentUser.Id)
            || (m.ReceiverId == currentUser.Id && m.SenderId == recieverId),m=>m.Timestamp,OrderDirection.Descending);
            var messageResult=messages.Select(message=>_mapper.Map<MessageResultDto>(message)).ToList();
            return messageResult;
        }


        public async Task<bool> SendMessageAsync(string recieverId, SendMessageDto sendMessageDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("current user not found");
            var reciever = await _userManager.FindByIdAsync(recieverId);
            if (reciever == null) throw new Exception("reciever not found");
            var message = _mapper.Map<Message>(sendMessageDto);
            message.SenderId = currentUser.Id;
            message.ReceiverId = reciever.Id;
            _unitOfWork.Message.Add(message);
            if (await _unitOfWork.SaveAsync() > 0) return true;
            return false;
        }
        public async Task<bool> UpdateMessageAsync(string messageId, SendMessageDto sendMessageDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("current user not found");
            var message = await _unitOfWork.Message.FindFirstAsync(m => m.Id == messageId);
            if (message == null) throw new Exception("message not found");
            _mapper.Map(sendMessageDto, message);
            _unitOfWork.Message.Update(message);
            if (await _unitOfWork.SaveAsync() > 0) return true;
            return false;
        }
        public async Task<bool> DeleteMessageAsync(string messageId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("current user not found");
            var message = await _unitOfWork.Message.FindFirstAsync(m => m.Id == messageId);
            if (message == null) throw new Exception("message not found");
            _unitOfWork.Message.Remove(message);
            if (await _unitOfWork.SaveAsync() > 0) return true;
            return false;
        }


        #endregion
    }
}
