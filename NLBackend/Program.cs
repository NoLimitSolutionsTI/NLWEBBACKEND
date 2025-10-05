using NLBackend.Models;
using NLBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// ===== PUERTO (Render) =====
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ===== REGISTRO DE SERVICIOS (todo ANTES de Build) =====
builder.Services.Configure<NLWebDatabaseSettings>(
    builder.Configuration.GetSection("NLWebDatabase"));

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("Smtp"));
builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();

builder.Services.AddSingleton<NLWebService>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== CONSTRUIR LA APP =====
var app = builder.Build();

// ===== PIPELINE =====
var runningOnRender = Environment.GetEnvironmentVariable("RENDER") == "true";

if (app.Environment.IsDevelopment() || runningOnRender)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

if (!runningOnRender)
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// Health check para Render
app.MapGet("/health", () => Results.Ok("OK"));

app.Run();