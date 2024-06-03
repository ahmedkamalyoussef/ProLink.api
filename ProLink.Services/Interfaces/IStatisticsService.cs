using ProLink.Application.Authentication;
using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatisticsDto> Statistics();
    }
}
