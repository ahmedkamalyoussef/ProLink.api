using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IRateService
    {
        Task<bool> AddRateAsync(string postId, RateDto rateDto);
        Task<bool> DeleteRateAsync(string userId);
    }
}
