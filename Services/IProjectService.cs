// Services/IProjectService.cs
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<Project> CreateAsync(Project toCreate);
        Task UpdateAsync(int id, Project toUpdate);
        Task DeleteAsync(int id);
    }
}
