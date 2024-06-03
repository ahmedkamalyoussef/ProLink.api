using ProLink.Data.Entities;

namespace ProLink.Application.DTOs
{
    public class UserResultDto
    {
        public string Id { get; set; }
        public bool IsFollowed { get; set; }
        public bool IsFriend { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? JopTitle { get; set; }
        public string? CV { get; set; }
        public int? FollowersCount {  get; set; }
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Rate { get; set; }
        public string? ProfilePicture { get; set; }
        public string? BackImage { get; set; }
        public List<string>? Skill { get; set; }
        public List<JobResultDto>? CompletedJobs { get; set; }
        public int? CompletedJobsCount { get; set; }
        public List<JobResultDto>? AcceptedJobs { get; set; }
        public int? AcceptedJobsCount { get; set; }
        public List<JobResultDto>? RefusedJobs { get; set; }
        public int? RefusedJobsCount { get; set; }


    }
}
