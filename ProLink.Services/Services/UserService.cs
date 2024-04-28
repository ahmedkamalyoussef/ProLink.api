using Microsoft.AspNetCore.Identity;
using ProLink.Data.Entities;
using ProLink.Application.Interfaces;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using Microsoft.Extensions.Configuration;

namespace ProLink.Application.Services
{
    public class UserService /*: IUserService*/
    {
        //    #region fields
        //    private readonly IUnitOfWork _unitOfWork;
        //    private readonly UserManager<User> _userManager;
        //    private readonly IHttpContextAccessor _contextAccessor;
        //    private readonly IMapper _mapper;
        //    private readonly IUserHelpers _userHelpers;
        //    private readonly SignInManager<User> _signInManager;
        //    #endregion

        //    #region ctor
        //    public UserService(IUnitOfWork unitOfWork,
        //        UserManager<User> userManager,
        //        IConfiguration config, IMapper mapper,
        //        IHttpContextAccessor contextAccessor,
        //        IUserHelpers userHelpers,
        //        SignInManager<User> signInManager

        //        )
        //    {
        //        _unitOfWork = unitOfWork;
        //        _userManager = userManager;
        //        _contextAccessor = contextAccessor;
        //        _mapper = mapper;
        //        _userHelpers = userHelpers;
        //        _signInManager = signInManager;


        //    }
        //    #endregion

        //    #region methods
        //    public async Task<bool> UpdateUserInfo(UserDto userDto)
        //    {
        //        var user = await _userHelpers.GetCurrentUserAsync();
        //        if (user == null)
        //            return false;
        //        try
        //        {
        //            user = _mapper.Map(userDto, user);
        //            _unitOfWork.User.Update(user);
        //            _unitOfWork.Save();
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    #endregion
        //}
    }
}
