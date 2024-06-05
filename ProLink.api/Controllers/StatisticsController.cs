using Microsoft.AspNetCore.Mvc;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatisticsAsync()
        {
            var result = await _statisticsService.StatisticsAsync();
            return Ok(result);
        }


        [HttpGet("user-jobs")]
        public async Task<IActionResult> GetUserJobsStatisticsAsync(string userId)
        {
            var result = await _statisticsService.UserJobsStatisticsAsync(userId);
            return Ok(result);
        }
    }
}
