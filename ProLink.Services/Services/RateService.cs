using Microsoft.AspNetCore.Identity;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Data.Consts;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class RateService : IRateService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IUserHelpers _userHelpers;

        #endregion

        #region ctor
        public RateService(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IUserHelpers userHelpers
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userHelpers = userHelpers;
        }
        #endregion


        public async Task<bool> AddRateAsync(string postId, RateDto rateDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("unAuthorized");
            var post = await _unitOfWork.Post.FindFirstAsync(p => p.Id == postId);
            if (post == null) throw new ArgumentNullException("user not found");
            if (post.UserId != currentUser.Id) throw new Exception("user not Authorized");
            if (post.Status != Status.Completed) throw new Exception("the post not completed yet");

            var rate = await _unitOfWork.Rate.FindFirstAsync(r => r.RaterId == currentUser.Id);
            if (rate == null)
            {
                var newRate = new Rate { RatedPostId = postId, RaterId = currentUser.Id, RateValue = rateDto.RateValue };
                _unitOfWork.Rate.Add(newRate);
                post.Rate=newRate;
            }
            else
            {
                rate.RateValue = rateDto.RateValue;
                _unitOfWork.Rate.Update(rate);
            }
            if (await _unitOfWork.SaveAsync() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteRateAsync(string rateId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("unAuthorized");
            var rate = await _unitOfWork.Rate.FindFirstAsync(f => f.Id == rateId);
            if (rate == null) return false;
            _unitOfWork.Rate.Remove(rate);
            if (await _unitOfWork.SaveAsync() > 0) return true;
            return false;
        }
    }
}
