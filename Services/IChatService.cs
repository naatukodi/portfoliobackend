// Services/IChatService.cs
using Portfolio.Api.Models;

namespace Portfolio.Api.Services;

public interface IChatService
{
    Task InitializeAsync();                // Call once at startup to index your files
    Task<ChatResponse> AskAsync(string q); // Handles query → answer
}
