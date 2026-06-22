using System.Text.Json.Serialization;
using Library_Management.Middleware;
using Library_Management.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Services (DI container) ──────────────────────────────────────────────

// TASK 2.1 — Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Book ↔ Author is a circular reference (Book.Author.Books contains
        // the same Book again). ReferenceHandler.IgnoreCycles breaks the
        // cycle during serialization instead of throwing.
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// TASK 1.4/1.5 + 2.2 — Register the book service for constructor injection
builder.Services.AddScoped<IBookService, BookService>();

// TASK 2.1 — Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   // ← add this line


var app = builder.Build();

// ── Middleware pipeline ───────────────────────────────────────────────────

// TASK 2.4 — Request logging, registered first so it wraps the whole pipeline
// (including downstream error handling) and still captures the final status code.
app.UseRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookLibrary API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// TASK 2.3 — Maps [ApiController]/[Route] attributed controllers, e.g. BooksController
app.MapControllers();

app.Run();