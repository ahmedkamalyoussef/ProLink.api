using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;
using ProLink.Application.Authentication;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion
        #region ctor
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region registration
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(user);
            if (result.Succeeded)
            {
                return Ok("Registration succeeded.");
            }
            return BadRequest(result.Errors);
        }
        #endregion


        [HttpPut("update-user-info")]
        public async Task<IActionResult> UpdateUserInfo(UserDto userDto)
        {
            var success = await _userService.UpdateUserInfo(userDto);

            if (!success)
            {
                return BadRequest("Failed to update user information.");
            }

            return Ok("user information updated successfully.");
        }
    }
}
