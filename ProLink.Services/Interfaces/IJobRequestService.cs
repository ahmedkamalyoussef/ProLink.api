using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IJobRequestService
    {
        Task<List<JobRequestDto>> GetJobRequistAsync();
        Task<bool> SendJobRequistAsync(string userId, string postId);
        Task<bool> AcceptJobAsync(string jobId);
        Task<bool> DeletePendingJobRequestAsync(string jobId);
        Task<bool> DeclinePendingJobRequestAsync(string jobId);
    }
}
