using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IFriendService
    {
        Task<List<UserResultDto>> GetFriendsAsync();

        Task<bool> DeleteFriendAsync(string friendId);
    }
}
