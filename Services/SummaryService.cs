// Services/SummaryService.cs
using System.Text.Json;
using Portfolio.Api.Models;

namespace Portfolio.Api.Services
{
    public class SummaryService : ISummaryService
    {
        // For demo, we load/save a JSON file under wwwroot/data/summary.json
        private readonly string _dataFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "summary.json");

        public async Task<Summary> GetAsync()
        {
            if (!File.Exists(_dataFile))
                throw new FileNotFoundException("Summary data not found");

            var json = await File.ReadAllTextAsync(_dataFile);
            return JsonSerializer.Deserialize<Summary>(json)!;
        }

        public async Task UpdateAsync(Summary updated)
        {
            var json = JsonSerializer.Serialize(updated, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_dataFile, json);
        }
    }
}
