using FluentValidation;
using FluentValidation.AspNetCore;
using MediCore.Api.Middlewares;
using MediCore.Api.Models;
using MediCore.Api.Services;
using MediCore.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi; // <-- Namespace oficial de OpenAPI v10
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

// ==========================================
// CONFIGURACIÓN DE SWAGGER (Swashbuckle v10+)
// ==========================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MediCore API", Version = "v1" });

    // 1. Definir el esquema de seguridad
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT. Pega tu token directamente aquí (Swagger añadirá el 'Bearer' por ti).",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // 2. Aplicar el candado globalmente usando el Transformer de v10
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        // ¡LA CLAVE ESTÁ AQUÍ! Usamos el indexador [] y pasamos el 'document'
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

// ==========================================
// INYECCIÓN DE DEPENDENCIAS (SERVICIOS Y BD)
// ==========================================
builder.Services.AddDbContext<MediCoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<ICobroService, CobroService>();

// ==========================================
// CONFIGURACIÓN DE SEGURIDAD JWT
// ==========================================
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

// ==========================================
// MIDDLEWARES DE ERROR
// ==========================================
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Escudo Global de Excepciones
app.UseExceptionHandler();

// Activar Swagger solo si estamos programando
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Candados de acceso
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();