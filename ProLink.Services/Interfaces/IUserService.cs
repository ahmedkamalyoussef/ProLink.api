using Microsoft.AspNetCore.Http;
using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResultDto> GetCurrentUserInfoAsync();
        Task<UserResultDto> GetUserByIdAsync(string id);
        Task<List<UserResultDto>> GetUsersByNameAsync(string name);
        Task<bool> UpdateUserInfoAsync(UserDto userDto);
        Task<bool> DeleteAccountAsync();
        //Task<bool> AddUserPictureAsync(IFormFile file);
        Task<bool> DeleteUserPictureAsync();
        Task<bool> UpdateUserPictureAsync(IFormFile? file);
        //Task<bool> AddUserCVAsync(IFormFile file);
        Task<string> GetUserPictureAsync();
        Task<bool> DeleteUserCVAsync();
        Task<bool> UpdateUserCVAsync(IFormFile? file);

        Task<string> GetUserCVAsync();
        Task<bool> DeleteUserBackImageAsync();
        Task<bool> UpdateUserBackImageAsync(IFormFile? file);

        Task<string> GetUserBackImageAsync();
        Task<List<JobRequestDto>> GetJobRequistAsync();
        Task<bool> SendJobRequistAsync(string userId,string postId);
        Task<bool> AcceptJobAsync(string jobId);
        Task<bool> DeletePendingJobRequestAsync(string jobId);
        Task<bool> DeclinePendingJobRequestAsync(string jobId);

        Task<List<FriendRequestDto>> GetFriendRequistsAsync();
        Task<bool> SendFriendAsync(string userId);
        Task<bool> AcceptFriendAsync(string friendRequestId);
        Task<bool> AcceptAllFriendsAsync();
        Task<bool> DeletePendingFriendAsync(string friendRequestId);
        Task<bool> DeclinePendingFriendAsync(string friendRequestId);
        Task<bool> AddSkillAsync(AddSkillDto addSkilltDto);
        Task<List<SkillDto>> GetCurrentUserSkillsAsync();
        Task<List<SkillDto>> GetUserSkillsByIdAsync(string id);

        Task<List<NotificationResultDto>> GetCurrentUserNotificationsAsync();
        Task<NotificationResultDto> GetNotificationByIdAsync(string notificationId);
        Task<bool> DeleteAllNotificationAsync();
        Task<bool> DeleteNotificationByIdAsync(string notificationId);

        Task<bool> UpdateSkillAsync(string skillId, AddSkillDto addSkillDto);
        Task<bool> DeleteSkillAsync(string skillId);
        Task<bool> AddRateAsync(string userId, RateDto rateDto);
        Task<bool> DeleteRateAsync(string userId);

        Task<bool> SendMessageAsync(string senderId,SendMessageDto sendMessageDto);
        Task<bool> UpdateMessageAsync(string messageId, SendMessageDto sendMessageDto);
        Task<bool> DeleteMessageAsync(string messageId);
    }
}
