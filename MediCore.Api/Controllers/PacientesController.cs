using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MediCore.Api.Models;
using MediCore.Api.DTOs;

namespace MediCore.Api.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly MediCoreContext _context;

        public PacientesController(MediCoreContext context)
        {
            _context = context;
        }

        // GET: api/Pacientes?pagina=1&recordsPorPagina=10
        [HttpGet]
        public async Task<IActionResult> ObtenerPacientes([FromQuery] int pagina = 1, [FromQuery] int recordsPorPagina = 10)
        {
            if (recordsPorPagina > 50) recordsPorPagina = 50;
            if (pagina < 1) pagina = 1;

            var query = _context.Pacientes.AsQueryable();
            var totalRegistros = await query.CountAsync();
            var paginasTotales = (int)Math.Ceiling(totalRegistros / (double)recordsPorPagina);

            var pacientes = await query
                .Skip((pagina - 1) * recordsPorPagina)
                .Take(recordsPorPagina)
                .Select(p => new PacienteResponseDto
                {
                    Id = p.Id,
                    NombreCompleto = p.NombreCompleto,
                    Telefono = p.Telefono
                })
                .ToListAsync();

            var response = new PaginacionResponseDto<PacienteResponseDto>
            {
                PaginaActual = pagina,
                PaginasTotales = paginasTotales,
                TotalRegistros = totalRegistros,
                Datos = pacientes
            };

            return Ok(response);
        }
        // GET: api/Pacientes/simular-caida
        [HttpGet("simular-caida")]
        public IActionResult SimularCaida()
        {
            // Simulamos que SQL Server se desconectó o dividimos entre cero por accidente
            throw new Exception("¡ERROR FATAL! Se desconectó la base de datos a nivel de hardware.");
        }

        // POST: api/Pacientes
        [HttpPost]
        public async Task<IActionResult> RegistrarPaciente([FromBody] PacienteRegistroDto dto)
        {
            if (_context.CurrentClinicaId == 0)
            {
                return Unauthorized(new { mensaje = "Token inválido o sin clínica asignada." });
            }

            var nuevoPaciente = new Paciente
            {
                NombreCompleto = dto.NombreCompleto,
                Telefono = dto.Telefono,

                ClinicaId = _context.CurrentClinicaId
            };

            _context.Pacientes.Add(nuevoPaciente);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Paciente registrado exitosamente.",
                id = nuevoPaciente.Id
            });
        }
        // DELETE: api/Pacientes/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == id);

            if (paciente == null) return NotFound(new { mensaje = "Paciente no encontrado." });

            // En lugar de _context.Pacientes.Remove(paciente), hacemos esto:
            paciente.Activo = false;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Expediente del paciente archivado correctamente." });
        }
    }
}