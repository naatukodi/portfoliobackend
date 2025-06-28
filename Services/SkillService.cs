// Services/SkillService.cs
using System.Text.Json;
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public class SkillService : ISkillService
    {
        private readonly string _dataFile =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "skills.json");

        private async Task<List<Skill>> LoadAsync()
        {
            if (!File.Exists(_dataFile))
                return new List<Skill>();

            var json = await File.ReadAllTextAsync(_dataFile);
            return JsonSerializer.Deserialize<List<Skill>>(json)!
                   ?? new List<Skill>();
        }

        private async Task SaveAsync(List<Skill> list)
        {
            var json = JsonSerializer.Serialize(list,
                new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_dataFile, json);
        }

        public async Task<IEnumerable<Skill>> GetAllAsync() =>
            await LoadAsync();

        public async Task<Skill?> GetByIdAsync(int id) =>
            (await LoadAsync()).FirstOrDefault(x => x.Id == id);

        public async Task<Skill> CreateAsync(Skill toCreate)
        {
            var list = await LoadAsync();
            toCreate.Id = list.Any() ? list.Max(x => x.Id) + 1 : 1;
            list.Add(toCreate);
            await SaveAsync(list);
            return toCreate;
        }

        public async Task UpdateAsync(int id, Skill toUpdate)
        {
            var list = await LoadAsync();
            var idx = list.FindIndex(x => x.Id == id);
            if (idx == -1) throw new KeyNotFoundException($"Skill {id} not found");
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
