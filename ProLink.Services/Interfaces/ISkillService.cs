using ProLink.Application.DTOs;

namespace ProLink.Application.Interfaces
{
    public interface ISkillService
    {
        Task<bool> AddSkillAsync(AddSkillDto addSkilltDto);
        Task<List<SkillDto>> GetCurrentUserSkillsAsync();
        Task<List<SkillDto>> GetUserSkillsByIdAsync(string id);
        Task<bool> UpdateSkillAsync(string skillId, AddSkillDto addSkillDto);
        Task<bool> DeleteSkillAsync(string skillId);
    }
}
