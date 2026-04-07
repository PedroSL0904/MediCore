using MediCore.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); // <--- Agrega esta línea
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <-- Swagger limpio y feliz, sin configuraciones extrañas

// === 1. CONFIGURACIÓN DE BASE DE DATOS ===
builder.Services.AddDbContext<MediCoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// === 2. CONFIGURACIÓN DEL CADENERO VIP (JWT Authentication) ===
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// === 3. EL FILTRO DE SEGURIDAD ===
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();