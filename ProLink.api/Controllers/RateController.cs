using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public RateController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion
        #region rate actions
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddRateAsync(string userId, RateDto rateDto)
        {
            var result = await _userService.AddRateAsync(userId, rateDto);
            return result ? Ok("rate has been added successfully") : BadRequest("faild to add rate");
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteRateAsync(string rateId)
        {
            var result = await _userService.DeleteRateAsync(rateId);
            return result ? Ok("rate has been deleted successfully") : BadRequest("faild to delete rate");
        }
        #endregion
    }
}
