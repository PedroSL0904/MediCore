using MediCore.Api.DTOs;
using MediCore.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MediCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MediCoreContext _context;
        private readonly IConfiguration _config;

        public AuthController(MediCoreContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("registrar")]
        public IActionResult RegistrarUsuario([FromBody] UsuarioRegistroDto dto)
        {
            string passwordEncriptada = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var nuevoUsuario = new Usuario
            {
                NombreCompleto = dto.Nombre,
                Email = dto.Correo,
                PasswordHash = passwordEncriptada,
                Rol = "Admin",
                ClinicaId = dto.ClinicaId,
                Activo = true
            };

            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();

            return Ok(new { mensaje = "Usuario registrado de forma segura." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UsuarioLoginDto dto)
        {
            // 1. Buscamos el correo
            var usuario = _context.Usuarios.IgnoreQueryFilters().FirstOrDefault(u => u.Email == dto.Correo);
            if (usuario == null) return Unauthorized(new { mensaje = "Credenciales incorrectas." });

            // 2. Verificamos la contraseña encriptada
            bool passwordValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);
            if (!passwordValida) return Unauthorized(new { mensaje = "Credenciales incorrectas." });

            // 3. Generamos el Token JWT
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("clinicaId", usuario.ClinicaId.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 4. Devolvemos el Token
            return Ok(new { token = tokenString });
        }
        [Authorize] // <--- ¡ESTE ES EL CANDADO!
        [HttpGet("perfil-vip")]
        public IActionResult ObtenerPerfilVip()
        {
            // Gracias al Token, la API ya sabe quién está haciendo la petición
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var clinicaId = User.FindFirst("clinicaId")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                mensaje = "¡Bienvenido a la zona VIP protegida!",
                usuarioLogueado = email,
                clinicaDelUsuario = clinicaId,
                nivelDeAcceso = rol
            });
        }
    }
}