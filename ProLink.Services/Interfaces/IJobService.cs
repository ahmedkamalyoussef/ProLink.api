using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface IJobService
    {
        Task<bool> AddJobAsync(JobDto post);
        Task<bool> UpdateJobAsync(string id, JobDto postDto);
        Task<bool> DeleteJobAsync(string id);
        Task<bool> CompleteAsync(string postId);
        Task<JobResultDto> GetJobByIdAsync(string id);
        Task<List<JobResultDto>> GetUserJobsAsync();
        Task<List<JobResultDto>> GetUserJobsByUserIdAsync(string id);
        Task<List<JobResultDto>> GetAllJobsAsync();
        Task<List<JobResultDto>> GetJobsByTitleAsync(string title);

    }
}
