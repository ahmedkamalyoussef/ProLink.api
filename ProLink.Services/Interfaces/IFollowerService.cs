using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IFollowerService
    {
        Task<List<UserResultDto>> GetFollowesAsync();
        Task<bool> FollowAsync(string userId);
        Task<bool> UnFollowAsync(string userId);
    }
}
