using AutoMapper;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class FriendService:IFriendService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;

        #endregion

        #region ctor
        public FriendService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IUserHelpers userHelpers
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userHelpers = userHelpers;
        }
        #endregion

        #region friends
        //public async Task<List<UserResultDto>> GetFriendsAsync()
        //{
        //    var currentUser = await _userHelpers.GetCurrentUserAsync();
        //    if (currentUser == null) throw new Exception("user not found");
        //    var friends = currentUser.Friends;
        //    var friendsResult = friends.Select(friend => _mapper.Map<UserResultDto>(friend)).ToList();
        //    return friendsResult;
        //}

        public async Task<List<UserResultDto>> GetFriendsAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var friends = currentUser.Friends;
            var friendsResult =  _mapper.Map<IEnumerable< UserResultDto>>(friends).ToList();
            return friendsResult;
        }

        public async Task<bool> DeleteFriendAsync(string friendId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var friend = currentUser.Friends.FirstOrDefault(friend => friend.Id == friendId);
            if (friend == null) throw new Exception("friend not found");
            currentUser.Friends.Remove(friend);
            _unitOfWork.User.Update(currentUser);
            friend.Friends.Remove(currentUser);
            _unitOfWork.User.Update(friend);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }
        #endregion
    }
}
