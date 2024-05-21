using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Application.Mail;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class FollowerService : IFollowerService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;
        private readonly IMailingService _mailingService;
        private readonly UserManager<User> _userManager;
        #endregion

        #region ctor
        public FollowerService(IUnitOfWork unitOfWork,
            IMapper mapper,
            UserManager<User> userManager,
            IUserHelpers userHelpers,
            IMailingService mailingService
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
            _mailingService = mailingService;
            _userManager = userManager;
        }
        #endregion

        public async Task<List<UserResultDto>> GetFollowesAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var followers = currentUser.Followers;
            var followersResult = followers.Select(follower => _mapper.Map<UserResultDto>(follower)).ToList();
            return followersResult;
        }
        public async Task<bool> FollowAsync(string userId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) return false;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            if(user.Followers.Contains(currentUser)) return true;
            await _unitOfWork.CreateTransactionAsync();
            try
            {
                user.Followers.Add(currentUser);
                _unitOfWork.Save();



                await _unitOfWork.CreateSavePointAsync("addfollower");

                var notification = new Notification
                {
                    Content = $"{currentUser.FirstName} {currentUser.LastName} just started following you",
                    Timestamp = DateTime.Now,
                    ReceiverId = user.Id
                };
                _unitOfWork.Notification.Add(notification);
                _unitOfWork.Save();

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackToSavePointAsync("addfollower");
                await _unitOfWork.CommitAsync();
                return false;
            }
            var message = new MailMessage(new string[] { user.Email }, "followers",
                $"{currentUser.FirstName} {currentUser.LastName} just started following you");
            _mailingService.SendMail(message);

            return true;
        }


        public async Task<bool> UnFollowAsync(string userId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) return false;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            user.Followers.Remove(currentUser);
            return _unitOfWork.Save() > 0;
        }
    }
}
