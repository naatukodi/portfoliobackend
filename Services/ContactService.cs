// Services/ContactService.cs
using System.Text.Json;
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public class ContactService : IContactService
    {
        private readonly string _dataFile =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "contacts.json");

        private async Task<List<Contact>> LoadAsync()
        {
            if (!File.Exists(_dataFile))
                return new List<Contact>();

            var json = await File.ReadAllTextAsync(_dataFile);
            return JsonSerializer.Deserialize<List<Contact>>(json)!
                   ?? new List<Contact>();
        }

        private async Task SaveAsync(List<Contact> list)
        {
            var json = JsonSerializer.Serialize(list,
                new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_dataFile, json);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync() =>
            await LoadAsync();

        public async Task<Contact?> GetByIdAsync(int id) =>
            (await LoadAsync()).FirstOrDefault(x => x.Id == id);

        public async Task<Contact> CreateAsync(Contact toCreate)
        {
            var list = await LoadAsync();
            toCreate.Id = list.Any() ? list.Max(x => x.Id) + 1 : 1;
            list.Add(toCreate);
            await SaveAsync(list);
            return toCreate;
        }

        public async Task UpdateAsync(int id, Contact toUpdate)
        {
            var list = await LoadAsync();
            var idx = list.FindIndex(x => x.Id == id);
            if (idx == -1) throw new KeyNotFoundException($"Contact {id} not found");
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
