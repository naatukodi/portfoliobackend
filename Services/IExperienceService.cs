// Services/IExperienceService.cs
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public interface IExperienceService
    {
        Task<IEnumerable<Experience>> GetAllAsync();
        Task<Experience?> GetByIdAsync(int id);
        Task<Experience> CreateAsync(Experience toCreate);
        Task UpdateAsync(int id, Experience toUpdate);
        Task DeleteAsync(int id);
    }
}
