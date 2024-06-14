using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IFriendRequestService
    {
        Task<List<FriendRequestDto>> GetFriendRequistsAsync();
        Task<bool> SendFriendAsync(string userId);
        Task<bool> AcceptFriendAsync(string friendRequestId);
        Task<bool> AcceptAllFriendsAsync();
        Task<bool> DeletePendingFriendAsync(string userId);
        Task<bool> DeclinePendingFriendAsync(string friendRequestId);
    }
}
