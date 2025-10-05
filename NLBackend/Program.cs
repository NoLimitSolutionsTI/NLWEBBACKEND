using NLBackend.Models;
using NLBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// CONFIGURACIÓN DEL SERVIDOR Y PUERTO
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
// CONSTRUCCIÓN DE LA APLICACIÓN
// ======================================================

var app = builder.Build();

// ======================================================
// CONFIGURACIÓN DEL PIPELINE HTTP
// ======================================================

// Habilita Swagger solo en Development o en Render (para debugging)
var runningOnRender = Environment.GetEnvironmentVariable("RENDER") == "true";

if (app.Environment.IsDevelopment() || runningOnRender)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuración de CORS
app.UseCors("AllowReactApp");

// Render ya maneja HTTPS externamente, por lo tanto evitamos la redirección interna
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
// EJECUCIÓN
// ======================================================

app.Run();