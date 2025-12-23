using BombermanServer.Data;
using BombermanServer.Data.Repositories;
using BombermanServer.Data.IRepositories;
using BombermanServer.Hubs;
using Microsoft.EntityFrameworkCore;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------
// 1. SERVÝSLERÝ EKLEME (Services Configuration)
// -------------------------------------------------------------------------

// A) Veritabaný Baðlantýsý (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// B) Repository'leri Kaydetme (Dependency Injection için)
// IUserRepository istendiðinde UserRepository verilecek.
builder.Services.AddScoped<IUserRepository, UserRepository>();

// C) SignalR Servisi
builder.Services.AddSignalR();

// D) CORS Politikasý (Unity Baðlantýsý Ýçin - Gevþetilmiþ Sürüm)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUnity",
        policyBuilder =>
        {
            policyBuilder.SetIsOriginAllowed(origin => true)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

// E) Swagger (API Dokümantasyonu)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------------------------------------------------------------
// 2. UYGULAMA PIPELINE'I (Middleware Pipeline)
// -------------------------------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS politikasýný uygula
app.UseCors("AllowUnity");

app.UseAuthorization();

app.MapControllers();

// SignalR Hub'ý /gameHub adresine eþle
app.MapHub<GameHub>("/gameHub");

app.Run();