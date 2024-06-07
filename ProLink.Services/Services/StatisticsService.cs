using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public StatisticsService(IUnitOfWork unitOfWork , UserManager<User> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<StatisticsDto> StatisticsAsync()
        {
            StatisticsDto statisticsDto = new StatisticsDto
            {
                UserCounts = await _unitOfWork.User.Count(),
                JobsCount = await _unitOfWork.Job.Count(),
                PostsCount = await _unitOfWork.Post.Count()
            };
            return statisticsDto;
        }

        public async Task<UserJobsStatisticsDto> UserJobsStatisticsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("user not found");
            UserJobsStatisticsDto userJobsStatisticsDto = new UserJobsStatisticsDto
            {
                CompletedJobs = _mapper.Map<IEnumerable<JobResultDto>>(user.CompletedJobs).ToList(),
                RefusedJobs = _mapper.Map<IEnumerable<JobResultDto>>(user.RefusedJobs).ToList(),
                CompletedJobsCount = user.CompletedJobs.Count(),
                RefusedJobsCount = user.RefusedJobs.Count()
            };
            return userJobsStatisticsDto;
        }
    }
}
