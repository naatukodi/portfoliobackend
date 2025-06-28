// Services/ExperienceService.cs
using System.Text.Json;
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly string _dataFile =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "experiences.json");

        private async Task<List<Experience>> LoadAsync()
        {
            if (!File.Exists(_dataFile))
                return new List<Experience>();

            var text = await File.ReadAllTextAsync(_dataFile);
            return JsonSerializer.Deserialize<List<Experience>>(text)!
                   ?? new List<Experience>();
        }

        private async Task SaveAsync(List<Experience> list)
        {
            var text = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_dataFile, text);
        }

        public async Task<IEnumerable<Experience>> GetAllAsync() =>
            await LoadAsync();

        public async Task<Experience?> GetByIdAsync(int id) =>
            (await LoadAsync()).FirstOrDefault(x => x.Id == id);

        public async Task<Experience> CreateAsync(Experience toCreate)
        {
            var list = await LoadAsync();
            toCreate.Id = list.Any() ? list.Max(x => x.Id) + 1 : 1;
            list.Add(toCreate);
            await SaveAsync(list);
            return toCreate;
        }

        public async Task UpdateAsync(int id, Experience toUpdate)
        {
            var list = await LoadAsync();
            var idx = list.FindIndex(x => x.Id == id);
            if (idx == -1) throw new KeyNotFoundException($"Experience {id} not found");
            toUpdate.Id = id;
            list[idx] = toUpdate;
            await SaveAsync(list);
        }

        public async Task DeleteAsync(int id)
        {
            var list = await LoadAsync();
            list.RemoveAll(x => x.Id == id);
            await SaveAsync(list);
        }
    }
}
