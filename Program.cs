using Portfolio.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Register application services
builder.Services.AddSingleton<ISummaryService, SummaryService>();
builder.Services.AddSingleton<IExperienceService, ExperienceService>();
builder.Services.AddSingleton<IEducationService, EducationService>();
builder.Services.AddSingleton<IProjectService, ProjectService>();

// Add controllers
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
