using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<StatisticsDto> Statistics()
        {
            StatisticsDto statisticsDto = new StatisticsDto
            {
                UserCounts = await _unitOfWork.User.Count(),
                JobsCount = await _unitOfWork.Job.Count(),
                PostsCount = await _unitOfWork.Post.Count()
            };
            return statisticsDto;
        }
    }
}
