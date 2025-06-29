// Services/ChatService.cs

using OpenAI.Embeddings;
using OpenAI.Chat;
using Portfolio.Api.Models;   // for ChatRequest/ChatResponse
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;

namespace Portfolio.Api.Services;

public class ChatService : IChatService
{
    private readonly HttpClient _openAiClient;
    private readonly EmbeddingClient _embeddings;
    private readonly ChatClient _chat;
    private readonly IVectorStore _vectors;
    private readonly string _dataFolder
        = Path.Combine(Directory.GetCurrentDirectory(), "Data");
    private const string ChatEndpoint = "https://api.openai.com/v1/chat/completions";
    private const string Model = "gpt-3.5-turbo";
    private readonly HttpClient _http;
    private readonly BlobContainerClient _container;


    public ChatService(
        IHttpClientFactory httpClientFactory,
        HttpClient http,
        EmbeddingClient embeddings,
        ChatClient chat,
        IVectorStore vectors,
        IConfiguration config)
    {
        _embeddings = embeddings;
        _chat = chat;
        _vectors = vectors;
        _http = http;
        _openAiClient = httpClientFactory.CreateClient("OpenAI");

        // initialize the blob container client
        var connStr = config["Azure:BlobConnectionString"]!;
        var container = config["Azure:BlobContainer"]!;
        _container = new BlobContainerClient(connStr, container);
    }

    public async Task InitializeAsync()
    {
        // create container if missing
        await _container.CreateIfNotExistsAsync();

        // list all blobs
        await foreach (var blobItem in _container.GetBlobsAsync())
        {
            var ext = Path.GetExtension(blobItem.Name).ToLowerInvariant();
            if (ext != ".txt" && ext != ".pdf" && ext != ".docx")
                continue;

            // download content to memory
            var blobClient = _container.GetBlobClient(blobItem.Name);
            using var ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);
            ms.Position = 0;

            // extract text
            string text = ext switch
            {
                ".txt" => Encoding.UTF8.GetString(ms.ToArray()),

                ".pdf" => ExtractPdfText(ms),

                ".docx" => ExtractWordText(ms),

                _ => string.Empty
            };

            // embed & index
            var embedResult = await _embeddings.GenerateEmbeddingAsync(text);
            var embedding = embedResult.Value;
            var vector = embedding.ToFloats().ToArray();

            await _vectors.IndexAsync(
                id: blobItem.Name,
                embedding: vector,
                text: text
            );
        }
    }

    private static string ExtractPdfText(Stream pdfStream)
    {
        var sb = new StringBuilder();
        using var doc = PdfDocument.Open(pdfStream);
        foreach (var page in doc.GetPages())
            sb.AppendLine(page.Text);
        return sb.ToString();
    }

    private static string ExtractWordText(Stream docxStream)
    {
        var sb = new StringBuilder();
        using var wordDoc = WordprocessingDocument.Open(docxStream, false);
        var body = wordDoc.MainDocumentPart?.Document.Body;
        if (body != null)
            sb.Append(body.InnerText);
        return sb.ToString();
    }


    public async Task<ChatResponse> AskAsync(string question)
    {
        // 1) embed question and fetch top contexts
        var qEmbedResult = await _embeddings.GenerateEmbeddingAsync(question);
        var qVector = qEmbedResult.Value.ToFloats().ToArray();
        var nearest = await _vectors.NearestAsync(qVector, 3);
        var contextText = string.Join("\n---\n", nearest.Select(n => n.text));

        // 2) build system + user message objects
        var system = new
        {
            role = "system",
            content = "You are a personal assistant who knows all my indexed data."
        };

        var userSb = new StringBuilder();
        userSb.AppendLine("Context:");
        userSb.AppendLine(contextText);
        userSb.AppendLine();
        userSb.AppendLine("Question:");
        userSb.AppendLine(question);

        var user = new
        {
            role = "user",
            content = userSb.ToString()
        };

        // 3) assemble the chat-completion payload
        var payload = new
        {
            model = Model,
            messages = new[] { system, user },
            temperature = 0.0,
            max_tokens = 300
        };

        var json = JsonSerializer.Serialize(payload);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        var resp = await _openAiClient.PostAsync("/v1/chat/completions", content);
        resp.EnsureSuccessStatusCode();

        // var json = JsonSerializer.Serialize(payload);
        // using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // // 4) send the request
        // var resp = await _http.PostAsync(ChatEndpoint, content);
        // resp.EnsureSuccessStatusCode();

        // 5) parse the first choice out of the JSON
        using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
        var answer = doc.RootElement
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString()!
                        .Trim();

        return new ChatResponse { Answer = answer };
    }

}
