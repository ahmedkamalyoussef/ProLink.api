namespace ProLink.Application.DTOs
{
    public class UserJobsStatisticsDto
    {
        public List<JobResultDto>? CompletedJobs { get; set; }
        public int? CompletedJobsCount { get; set; }
        public List<JobResultDto>? RefusedJobs { get; set; }
        public int? RefusedJobsCount { get; set; }
    }
}
