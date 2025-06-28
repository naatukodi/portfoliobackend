// Services/IContactService.cs
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact?> GetByIdAsync(int id);
        Task<Contact> CreateAsync(Contact toCreate);
        Task UpdateAsync(int id, Contact toUpdate);
        Task DeleteAsync(int id);
    }
}
