using NLBackend.Models;
using NLBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Asegura que la app escuche en el puerto que Render inyecta
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services
builder.Services.Configure<NLWebDatabaseSettings>(
    builder.Configuration.GetSection("NLWebDatabase"));

builder.Services.AddSingleton<NLWebService>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger en Dev (déjalo así o habilítalo también en Prod si quieres)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

// 2) Evita forzar HTTPS dentro del contenedor en Render
var runningOnRender = Environment.GetEnvironmentVariable("RENDER") == "true";
if (!runningOnRender)
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// 3) Endpoint de health para Render
app.MapGet("/health", () => Results.Ok("OK"));

app.Run();