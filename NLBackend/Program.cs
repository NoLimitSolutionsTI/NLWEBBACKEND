using NLBackend.Models;
using NLBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// CONFIGURACI�N DEL SERVIDOR Y PUERTO
// ======================================================

// Render inyecta el puerto mediante la variable de entorno PORT
// Si no existe (por ejemplo, al correr localmente), usa 8080 por defecto.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ======================================================
// REGISTRO DE SERVICIOS
// ======================================================

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

// ======================================================
// CONSTRUCCI�N DE LA APLICACI�N
// ======================================================

var app = builder.Build();

// ======================================================
// CONFIGURACI�N DEL PIPELINE HTTP
// ======================================================

// Habilita Swagger solo en Development o en Render (para debugging)
var runningOnRender = Environment.GetEnvironmentVariable("RENDER") == "true";

if (app.Environment.IsDevelopment() || runningOnRender)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuraci�n de CORS
app.UseCors("AllowReactApp");

// Render ya maneja HTTPS externamente, por lo tanto evitamos la redirecci�n interna
if (!runningOnRender)
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

// ======================================================
// ENDPOINTS
// ======================================================

app.MapControllers();

// Health Check para Render (responde 200 OK)
app.MapGet("/health", () => Results.Ok("OK"));

// ======================================================
// EJECUCI�N
// ======================================================

app.Run();