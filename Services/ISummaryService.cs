// Services/ISummaryService.cs
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public interface ISummaryService
    {
        Task<Summary> GetAsync();
        Task UpdateAsync(Summary updated);
    }
}
