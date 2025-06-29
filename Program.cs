using Portfolio.Api.Services;
using OpenAI;
using OpenAI.Embeddings;
using OpenAI.Chat;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// load API key
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddHttpClient<ChatService>(c =>
{
    c.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", builder.Configuration["OpenAI:ApiKey"]);
});

// 7) OpenAI and Google CSE HTTP clients
builder.Services.AddHttpClient("OpenAI", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", builder.Configuration["OpenAI:ApiKey"]);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

// get the API key
var apiKey = builder.Configuration["OpenAI:ApiKey"]!;

// register the embedding client
builder.Services.AddSingleton(_ =>
    new EmbeddingClient(
        model: "text-embedding-3-small",
        apiKey: apiKey
    ));

// register the chat client
builder.Services.AddSingleton(_ =>
    new ChatClient(
        model: "gpt-3.5-turbo",
        apiKey: apiKey
    ));

// register services
builder.Services.AddSingleton<IVectorStore, InMemoryVectorStore>();
builder.Services.AddSingleton<IChatService, ChatService>();

// Register application services
builder.Services.AddSingleton<ISummaryService, SummaryService>();
builder.Services.AddSingleton<IExperienceService, ExperienceService>();
builder.Services.AddSingleton<IEducationService, EducationService>();
builder.Services.AddSingleton<IProjectService, ProjectService>();
builder.Services.AddSingleton<ISkillService, SkillService>();
builder.Services.AddSingleton<IContactService, ContactService>();

// Add controllers
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ← call InitializeAsync here, before mapping controllers or starting the server
using (var scope = app.Services.CreateScope())
{
    var chatSvc = scope.ServiceProvider.GetRequiredService<IChatService>();
    await chatSvc.InitializeAsync();
}

// Enable Swagger UI at application root
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API V1");
    c.RoutePrefix = string.Empty; // Serve Swagger UI at application root
});

app.UseHttpsRedirection();

// Map controller endpoints
app.MapControllers();

app.Run();
