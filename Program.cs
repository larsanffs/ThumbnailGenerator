using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

int[] widths = { 32, 64, 128, 256, 512, 1024 };

app.MapGet("thumbnails/{id}", (string id, [FromQuery] string width) =>
{
    var path = Path.Combine(Path.GetTempPath(), id);
    if (!System.IO.File.Exists(path))
    {
        return Results.NotFound();
    }

    var stream = System.IO.File.OpenRead(path);
    return Results.File(stream, "image/jpeg");
});

app.MapPost("thumbnails", async (IFormFile? file) =>
{
    if (file == null)
    {
        return Results.BadRequest("No file uploaded");
    }

    if (!file.IsValidImage())
    {
        return Results.BadRequest("Invalid file type");
    }

    var id = Path.GetFileName(Path.GetTempFileName());
    var path = Path.GetTempPath();
    var filePath = Path.Combine(path, id);

    using (var stream = System.IO.File.Create(filePath))
    {
        await file.CopyToAsync(stream);
    }
    return Results.Ok(new { filePath });
}).DisableAntiforgery();


app.Run();