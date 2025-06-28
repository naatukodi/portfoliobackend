// Services/ISkillService.cs
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public interface ISkillService
    {
        Task<IEnumerable<Skill>> GetAllAsync();
        Task<Skill?> GetByIdAsync(int id);
        Task<Skill> CreateAsync(Skill toCreate);
        Task UpdateAsync(int id, Skill toUpdate);
        Task DeleteAsync(int id);
    }
}
