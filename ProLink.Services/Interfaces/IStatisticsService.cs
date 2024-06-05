using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatisticsDto> StatisticsAsync();
        Task<UserJobsStatisticsDto> UserJobsStatisticsAsync(string userId);
    }
}
