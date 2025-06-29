// Services/IVectorStore.cs

namespace Portfolio.Api.Services;

public interface IVectorStore
{
    Task IndexAsync(string id, float[] embedding, string text);
    Task<List<(string id, float similarity, string text)>> NearestAsync(float[] queryEmbedding, int topK = 3);
}
