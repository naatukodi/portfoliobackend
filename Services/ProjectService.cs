// Services/ProjectService.cs
using System.Text.Json;
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public class ProjectService : IProjectService
    {
        private readonly string _dataFile =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "projects.json");

        private async Task<List<Project>> LoadAsync()
        {
            if (!File.Exists(_dataFile))
                return new List<Project>();

            var json = await File.ReadAllTextAsync(_dataFile);
            return JsonSerializer
                   .Deserialize<List<Project>>(json)!
                   ?? new List<Project>();
        }

        private async Task SaveAsync(List<Project> list)
        {
            var json = JsonSerializer.Serialize(list,
                new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_dataFile, json);
        }

        public async Task<IEnumerable<Project>> GetAllAsync() =>
            await LoadAsync();

        public async Task<Project?> GetByIdAsync(int id) =>
            (await LoadAsync()).FirstOrDefault(x => x.Id == id);

        public async Task<Project> CreateAsync(Project toCreate)
        {
            var list = await LoadAsync();
            toCreate.Id = list.Any() ? list.Max(x => x.Id) + 1 : 1;
            list.Add(toCreate);
            await SaveAsync(list);
            return toCreate;
        }

        public async Task UpdateAsync(int id, Project toUpdate)
        {
            var list = await LoadAsync();
            var idx = list.FindIndex(x => x.Id == id);
            if (idx == -1) throw new KeyNotFoundException($"Project {id} not found");
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
