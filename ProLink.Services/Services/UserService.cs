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
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            var user = await _userManager.FindByIdAsync(id);
            var userResult = _mapper.Map<UserResultDto>(user);
            var friendRequests = await _unitOfWork.FriendRequest.FindFirstAsync(f=>(f.SenderId==user.Id&&f.ReceiverId==currentUser.Id)||
            (f.ReceiverId == user.Id && f.SenderId == currentUser.Id));

            var userfollower=_unitOfWork.UserFollower.FindFirstAsync(uf=>uf.FollowerId==currentUser.Id&&uf.UserId==user.Id);

            if (userfollower!=null) userResult.IsFollowed = true;
            else userResult.IsFollowed = false;

            var userFriend = await _unitOfWork.UserFriend.FindFirstAsync(uf =>
            (uf.UserId == currentUser.Id && uf.FriendId == user.Id)
            || (uf.UserId == user.Id && uf.FriendId == currentUser.Id));

            if (userFriend!=null) userResult.IsFriend = true;
            else userResult.IsFriend = false;
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
                await _unitOfWork.SaveAsync();
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
        

        public async Task<bool> DeleteUserPictureAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldPicture = user.ProfilePicture;
            user.ProfilePicture = null;
            _unitOfWork.User.Update(user);
            if (await _unitOfWork.SaveAsync() > 0)
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
            if (await _unitOfWork.SaveAsync() > 0)
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

        public async Task<bool> DeleteUserCVAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) return false;
            var oldCV = user.CV;
            user.CV = null;
            _unitOfWork.User.Update(user);
            if (await _unitOfWork.SaveAsync() > 0)
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
            if (await _unitOfWork.SaveAsync() > 0)
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
            if (await _unitOfWork.SaveAsync() > 0)
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
            if (await _unitOfWork.SaveAsync() > 0)
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
