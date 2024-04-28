using Castle.Core.Resource;
using Microsoft.AspNetCore.Identity;
using ProLink.Application.Authentication;
using ProLink.Application.DTOs;
using ProLink.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProLink.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UpdateUserInfo(UserDto userDto);
        Task<IdentityResult> Register(RegisterUser registerUser);
    }
}
