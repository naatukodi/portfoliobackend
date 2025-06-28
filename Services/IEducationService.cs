// Services/IEducationService.cs
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public interface IEducationService
    {
        Task<IEnumerable<Education>> GetAllAsync();
        Task<Education?> GetByIdAsync(int id);
        Task<Education> CreateAsync(Education toCreate);
        Task UpdateAsync(int id, Education toUpdate);
        Task DeleteAsync(int id);
    }
}
