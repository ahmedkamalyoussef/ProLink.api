using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProLink.Application.DTOs;
using ProLink.Application.Interfaces;

namespace ProLink.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region fields
        private readonly IUserService _userService;
        #endregion
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        //[HttpPut("update-user-info")]
        //public async Task<IActionResult> UpdateUserInfo(UserDto userDto)
        //{
        //    var success = await _userService.UpdateUserInfo(userDto);

        //    if (!success)
        //    {
        //        return BadRequest("Failed to update customer information.");
        //    }

        //    return Ok("Customer information updated successfully.");
        //}
    }
}
