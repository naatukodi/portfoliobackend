// Services/InMemoryVectorStore.cs
using System.Collections.Concurrent;

namespace Portfolio.Api.Services;

public class InMemoryVectorStore : IVectorStore
{
    // id → (embedding, text)
    private readonly ConcurrentDictionary<string, (float[] embedding, string text)> _store
        = new ConcurrentDictionary<string, (float[], string)>();

    public Task IndexAsync(string id, float[] embedding, string text)
    {
        _store[id] = (embedding, text);
        return Task.CompletedTask;
    }

    public Task<List<(string id, float similarity, string text)>> NearestAsync(float[] queryEmbedding, int topK = 3)
    {
        var results = _store
            .Select(kvp =>
            {
                var sim = CosineSimilarity(queryEmbedding, kvp.Value.embedding);
                return (id: kvp.Key, similarity: sim, text: kvp.Value.text);
            })
            .OrderByDescending(x => x.similarity)
            .Take(topK)
            .ToList();
        return Task.FromResult(results);
    }

    private static float CosineSimilarity(float[] a, float[] b)
    {
        float dot = 0, magA = 0, magB = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }
        return dot / ((float)Math.Sqrt(magA) * (float)Math.Sqrt(magB) + 1e-6f);
    }
}
