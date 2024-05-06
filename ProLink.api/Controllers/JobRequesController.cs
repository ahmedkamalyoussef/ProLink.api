using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRequesController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public JobRequesController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region job Request actions

        [Authorize]
        [HttpGet("Get-jobRequests")]
        public async Task<IActionResult> GetjobRequestAsync()
        {
            var result = await _userService.GetJobRequistAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpPost("add-jobRequest")]
        public async Task<IActionResult> SendjobRequestAsync(string userId, string postId)
        {
            var result = await _userService.SendJobRequistAsync(userId, postId);
            return result ? Ok("jobRequest has been sent successfully") : BadRequest("faild to send jobRequest");
        }

        [Authorize]
        [HttpPut("accept-jobRequest")]
        public async Task<IActionResult> AcceptJobAsync(string jobRequestId)
        {
            var result = await _userService.AcceptJobAsync(jobRequestId);
            return result ? Ok("job Request has been accepted successfully") : BadRequest("faild to accept job Request");
        }

        [Authorize]
        [HttpDelete("delete-jobRequest")]
        public async Task<IActionResult> DeletejobRequestAsync(string requestId)
        {
            var result = await _userService.DeletePendingJobRequestAsync(requestId);
            return result ? Ok("jobRequest has been deleted successfully") : BadRequest("faild to delete jobRequest");
        }

        [Authorize]
        [HttpPut("decline-jobRequest")]
        public async Task<IActionResult> DeclinejobRequestAsync(string requestId)
        {
            var result = await _userService.DeclinePendingJobRequestAsync(requestId);
            return result ? Ok("jobRequest has been Declined successfully") : BadRequest("faild to Declined jobRequest");
        }
        #endregion
    }
}
