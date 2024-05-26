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

        public async Task<List<UserResultDto>> GetFriendsAsync()
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var userFriends = await _unitOfWork.UserFriend.FindAsync(uf=>uf.UserId == currentUser.Id);
            var friends = userFriends.Select(uf => uf.Friend);
            var friendsResult =  _mapper.Map<IEnumerable< UserResultDto>>(friends).ToList();
            return friendsResult;
        }

        public async Task<bool> DeleteFriendAsync(string friendId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new Exception("user not found");
            var userFriend = await _unitOfWork.UserFriend.FindFirstAsync(uf=>uf.FriendId == friendId);
            if (userFriend == null) throw new Exception("friend not found");
            _unitOfWork.UserFriend.Remove(userFriend);
            _unitOfWork.UserFriend.Update(userFriend);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }
        #endregion
    }
}
