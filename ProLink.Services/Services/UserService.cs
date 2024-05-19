using Microsoft.AspNetCore.Identity;
using ProLink.Data.Entities;
using ProLink.Application.Interfaces;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Infrastructure.IGenericRepository_IUOW;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ProLink.Application.Consts;

namespace ProLink.Application.Services
{
    public class UserService : IUserService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;

        #endregion

        #region ctor
        public UserService(IUnitOfWork unitOfWork,
            UserManager<User> userManager, IMapper mapper,
            IUserHelpers userHelpers)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _userHelpers = userHelpers;
        }
        #endregion

        #region user methods

        public async Task<UserResultDto> GetCurrentUserInfoAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new Exception("User not found.");
            var user = _mapper.Map<UserResultDto>(currentUser);
            return user;
        }
        public async Task<UserResultDto> GetUserByIdAsync(string id)
        {   
            var user = await _userManager.FindByIdAsync(id);
            var userResult = _mapper.Map<UserResultDto>(user);
            return userResult;
        }

        public async Task<List<UserResultDto>> GetUsersByNameAsync(string name)
        {
            var users = await _unitOfWork.User.FindAsync(u => u.FirstName.Contains(name) || u.LastName.Contains(name));
            var usersResult = users.Select(user => _mapper.Map<UserResultDto>(user));
            return usersResult.ToList();
        }

        public async Task<bool> UpdateUserInfoAsync(UserDto userDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null)
                throw new ArgumentNullException("user not found");
            try
            {
                currentUser = _mapper.Map(userDto, currentUser);
                _unitOfWork.User.Update(currentUser);
                _unitOfWork.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAccountAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
            {
                return false;
            }
            await _userHelpers.DeleteFileAsync(user.ProfilePicture, ConstsFiles.Profile);
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

       

        #endregion

        

        #region file handlling
        //public async Task<bool> AddUserPictureAsync(IFormFile file)
        //{
        //    var user = await _userHelpers.GetCurrentUserAsync();
        //    if (user == null) return false;
        //    var picture = await _userHelpers.AddFileAsync(file, ConstsFiles.Profile);
        //    if (picture != null)
        //        user.ProfilePicture = picture;
        //    _unitOfWork.User.Update(user);
        //    if (_unitOfWork.Save() > 0) return true;
        //    return false;
        //}

        public async Task<bool> DeleteUserPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldPicture = user.ProfilePicture;
            user.ProfilePicture = null;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteFileAsync(oldPicture, ConstsFiles.Profile);
            return false;
        }

        public async Task<bool> UpdateUserPictureAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var newPicture = await _userHelpers.AddFileAsync(file, ConstsFiles.Profile);
            var oldPicture = user.ProfilePicture;
            user.ProfilePicture = newPicture;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
            {

                if (!oldPicture.IsNullOrEmpty())
                    return await _userHelpers.DeleteFileAsync(oldPicture, ConstsFiles.Profile);
                return true;
            }
            await _userHelpers.DeleteFileAsync(newPicture, ConstsFiles.Profile);
            return false;
        }

        public async Task<string> GetUserPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("User not found");
            else if (user.ProfilePicture.IsNullOrEmpty())
                throw new Exception("User dont have profile picture");
            return user.ProfilePicture;
        }


        //public async Task<bool> AddUserCVAsync(IFormFile file)
        //{
        //    var user = await _userHelpers.GetCurrentUserAsync();
        //    if (user == null) return false;
        //    var CV = await _userHelpers.AddFileAsync(file, ConstsFiles.CV);
        //    if (CV != null)
        //        user.CV = CV;
        //    _unitOfWork.User.Update(user);
        //    if (_unitOfWork.Save() > 0) return true;
        //    await _userHelpers.DeleteFileAsync(CV, ConstsFiles.CV);    
        //    return false;
        //}

        public async Task<bool> DeleteUserCVAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldCV = user.CV;
            user.CV = null;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteFileAsync(oldCV, ConstsFiles.CV);
            return false;
        }

        public async Task<bool> UpdateUserCVAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var newCV = await _userHelpers.AddFileAsync(file, ConstsFiles.CV);
            var oldCV = user.CV;
            user.CV = newCV;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
            {
                if (!oldCV.IsNullOrEmpty())
                    return await _userHelpers.DeleteFileAsync(oldCV, ConstsFiles.CV);
                return true;
            }
            await _userHelpers.DeleteFileAsync(newCV, ConstsFiles.CV);
            return false;
        }

        public async Task<string> GetUserCVAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("User not found");
            else if (user.CV.IsNullOrEmpty())
                throw new Exception("User dont have CV");
            return user.CV;
        }




        public async Task<bool> DeleteUserBackImageAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldBackImage = user.BackImage;
            user.BackImage = null;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
                return await _userHelpers.DeleteFileAsync(oldBackImage, ConstsFiles.BackImage);
            return false;
        }

        public async Task<bool> UpdateUserBackImageAsync(IFormFile? file)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var newBackImage = await _userHelpers.AddFileAsync(file, ConstsFiles.BackImage);
            var oldBackImage = user.BackImage;
            user.BackImage = newBackImage;
            _unitOfWork.User.Update(user);
            if (_unitOfWork.Save() > 0)
            {
                if (!oldBackImage.IsNullOrEmpty())
                    return await _userHelpers.DeleteFileAsync(oldBackImage, ConstsFiles.BackImage);
                return true;
            }
            await _userHelpers.DeleteFileAsync(newBackImage, ConstsFiles.BackImage);
            return false;
        }

        public async Task<string> GetUserBackImageAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null)
                throw new Exception("User not found");
            else if (user.BackImage.IsNullOrEmpty())
                throw new Exception("User dont have BackImage");
            return user.BackImage;
        }




        #endregion

        

        

        

        

        
    }
}
