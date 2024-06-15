using AutoMapper;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Data.Consts;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class NotificationService: INotificationService
    {

        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;

        #endregion

        #region ctor
        public NotificationService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserHelpers userHelpers
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
        }
        #endregion

        #region Notification
        public async Task<List<NotificationResultDto>> GetCurrentUserNotificationsAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");

            var notifications = await _unitOfWork.Notification
                .FindAsync(n => n.ReceiverId == currentUser.Id, n => n.Timestamp, OrderDirection.Descending);

            // Collect all AboutUserIds to fetch them in a single query
            var aboutUserIds = notifications
                .Where(n => n.AboutUserId != null)
                .Select(n => n.AboutUserId)
                .Distinct()
                .ToList();

            // Fetch all related users in a single query
            var aboutUsers = await _unitOfWork.User
                .FindAsync(u => aboutUserIds.Contains(u.Id));

            // Create a dictionary for quick lookup
            var aboutUserDictionary = aboutUsers.ToDictionary(u => u.Id);

            // Map notifications to result DTOs
            var notificationResultList = notifications.Select(notification =>
            {
                var notificationResult = _mapper.Map<NotificationResultDto>(notification);

                if (notification.AboutUserId != null && aboutUserDictionary.TryGetValue(notification.AboutUserId, out var aboutUser))
                {
                    notificationResult.Sender = _mapper.Map<UserPostResultDTO>(aboutUser);
                }

                return notificationResult;
            }).ToList();

            return notificationResultList;
        }


        public async Task<NotificationResultDto> GetNotificationByIdAsync(string notificationId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var notification = await _unitOfWork.Notification.FindFirstAsync(n => n.Id == notificationId);
            if (notification.ReceiverId != currentUser.Id) throw new Exception("cant access");
            var notificationResult = _mapper.Map<NotificationResultDto>(notification);
            if(notification.AboutUserId != null) 
                notificationResult.Sender=_mapper.Map<UserPostResultDTO>
                    ( await _unitOfWork.Notification.FindFirstAsync(n=>n.AboutUserId==notification.AboutUserId));
            return notificationResult;
        }

        public async Task<bool> DeleteNotificationByIdAsync(string notificationId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var notification = await _unitOfWork.Notification.FindFirstAsync(n => n.Id == notificationId);
            if (notification.ReceiverId != currentUser.Id) throw new Exception("cant access");
            _unitOfWork.Notification.Remove(notification);
            if (await _unitOfWork.SaveAsync() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteAllNotificationAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var notifications = await _unitOfWork.Notification.FindAsync(n => n.ReceiverId == currentUser.Id);
            _unitOfWork.Notification.RemoveRange(notifications);
            if (await _unitOfWork.SaveAsync() > 0) return true;
            return false;
        }

        #endregion
    }
}
