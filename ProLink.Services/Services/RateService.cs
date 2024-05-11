using Microsoft.AspNetCore.Identity;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class RateService:IRateService
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


        public async Task<bool> AddRateAsync(string userId, RateDto rateDto)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("unAuthorized");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new ArgumentNullException("user not found");
            var rate = await _unitOfWork.Rate.FindFirstAsync(r => r.RaterId == currentUser.Id);
            if (rate == null)
                _unitOfWork.Rate.Add(new Rate { RatedId = userId, RaterId = currentUser.Id, RateValue = rateDto.RateValue });
            else
            {
                rate.RateValue = rateDto.RateValue;
                _unitOfWork.Rate.Update(rate);
            }
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteRateAsync(string rateId)
        {
            var currentUser = await _userHelpers.GetCurrentUserAsync();
            if (currentUser == null) throw new UnauthorizedAccessException("unAuthorized");
            var rate = await _unitOfWork.Rate.FindFirstAsync(f => f.Id == rateId);
            if (rate == null) return false;
            _unitOfWork.Rate.Remove(rate);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }
    }
}
