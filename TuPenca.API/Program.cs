using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TuPenca.Application.Interfaces.Services;
using TuPenca.Application.Services;
using TuPenca.Domain.Entities;
using TuPenca.Domain.Interfaces;
using TuPenca.Domain.Interfaces.Repositories;
using TuPenca.Infrastructure.Data;
using TuPenca.Infrastructure.Data.Repositories;
using TuPenca.Infrastructure.Interfaces.Providers;
using TuPenca.Infrastructure.Middleware;
using TuPenca.Infrastructure.Providers;
// using TuPenca.Infrastructure.Data;
// revisar si es necesario

var builder = WebApplication.CreateBuilder(args);

// ─── Base de datos ───────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ─── Autenticación JWT ────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// ─── Multi-tenancy ────────────────────────────────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISitioProvider, SitioProvider>();

// ─── Repositorios, Service y Unit of Work ─────────────────────────
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();
builder.Services.AddScoped<IPlantillaPencaRepository, PlantillaPencaRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ISitioService, SitioService>();
builder.Services.AddScoped<IEventoDeportivoService, EventoDeportivoService>();
builder.Services.AddScoped<IPlantillaPencaService, PlantillaPencaService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ─── AutoMapper ───────────────────────────────────────────────
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 
//revisar pq da error

// ─── SignalR ──────────────────────────────────────────────────
builder.Services.AddSignalR(); // revisar la logica

// ─── Controllers + Swagger ────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ─── CORS ─────────────────────────────────────────────────────
// Permite que el frontend y la app móvil consuman la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ─── Servicios de Application ────────────────────────────────
// Acá vas a ir registrando tus servicios a medida que los creés
// Ejemplo:
// builder.Services.AddScoped<IPencaService, PencaService>();

var app = builder.Build();

// ─── Middleware ───────────────────────────────────────────────
app.UseMiddleware<SitioResolverMiddleware>();

// ─── Middleware pipeline ──────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();   // ⚠️ siempre antes de Authorization
app.UseAuthorization();

app.MapControllers();

// ─── SignalR Hubs ─────────────────────────────────────────────
// app.MapHub<ResultadosHub>("/hubs/resultados");

app.Run();