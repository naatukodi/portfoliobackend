namespace Portfolio.Api.Models;

// Models/ChatRequest.cs
public class ChatRequest
{
    public string Question { get; set; } = default!;
}


// Models/ChatResponse.cs
public class ChatResponse
{
    public string Answer { get; set; } = default!;
}
