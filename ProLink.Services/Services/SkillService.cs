using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProLink.Application.DTOs;
using ProLink.Application.Helpers;
using ProLink.Application.Interfaces;
using ProLink.Application.Mail;
using ProLink.Data.Entities;
using ProLink.Infrastructure.IGenericRepository_IUOW;

namespace ProLink.Application.Services
{
    public class SkillService:ISkillService
    {
        #region fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserHelpers _userHelpers;

        #endregion

        #region ctor
        public SkillService(IUnitOfWork unitOfWork,
            UserManager<User> userManager, IMapper mapper,
            IUserHelpers userHelpers)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _userHelpers = userHelpers;
        }
        #endregion

        #region skill methods
        public async Task<bool> AddSkillAsync(AddSkillDto addSkilltDto)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) throw new Exception("user not found");
            Skill skill = _mapper.Map<Skill>(addSkilltDto);
            skill.User = user;
            _unitOfWork.Skill.Add(skill);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<List<SkillDto>> GetCurrentUserSkillsAsync()
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) throw new Exception("user not found");
            var skills = await _unitOfWork.Skill.GetAllAsync();
            var skillDtos = _mapper.Map<List<SkillDto>>(skills);
            return skillDtos;
        }



        public async Task<List<SkillDto>> GetUserSkillsByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new Exception("user not found");
            var skills = await _unitOfWork.Skill.FindAsync(s => s.UserId == id);
            var skillDtos = _mapper.Map<List<SkillDto>>(skills);
            return skillDtos;
        }

        public async Task<bool> UpdateSkillAsync(string skillId, AddSkillDto addSkillDto)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) throw new Exception("user not found");
            var oldSkill = await _unitOfWork.Skill.FindFirstAsync(s => s.SkillId == skillId);
            if (oldSkill == null) throw new Exception("skill doesnt exist");
            if (oldSkill.UserId != user.Id) throw new UnauthorizedAccessException("cant update some one skill");
            _mapper.Map(addSkillDto, oldSkill);
            _unitOfWork.Skill.Update(oldSkill);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }

        public async Task<bool> DeleteSkillAsync(string skillId)
        {
            var user = await _userHelpers.GetCurrentUserAsync();
            if (user == null) throw new Exception("user not found");
            var skill = await _unitOfWork.Skill.FindFirstAsync(s => s.SkillId == skillId);
            if (skill == null) throw new Exception("skill doesnt exist");
            if (skill.UserId != user.Id) throw new UnauthorizedAccessException("cant delete some one skill");
            _unitOfWork.Skill.Remove(skill);
            if (_unitOfWork.Save() > 0) return true;
            return false;
        }
        #endregion
    }
}
