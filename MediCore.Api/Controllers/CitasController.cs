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
    public class CitasController : ControllerBase
    {
        private readonly MediCoreContext _context;

        public CitasController(MediCoreContext context)
        {
            _context = context;
        }

        // GET: api/Citas
        [HttpGet]
        public async Task<IActionResult> ObtenerCitas()
        {
            // El Filtro Global de la Clínica ya está actuando aquí.
            // Usamos Include() para traer los datos relacionados y no solo sus IDs.
            var citas = await _context.CitaMedicas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Select(c => new
                {
                    Id = c.Id,
                    Paciente = c.Paciente.NombreCompleto,
                    Medico = c.Medico.NombreCompleto,
                    Fecha = c.FechaHora,
                    Motivo = c.MotivoConsulta,
                    Estado = c.Estado
                })
                .ToListAsync();

            return Ok(citas);
        }

        // POST: api/Citas
        [HttpPost]
        public async Task<IActionResult> AgendarCita([FromBody] CitaRegistroDto dto)
        {
            // 1. Verificamos que el paciente exista y pertenezca a tu clínica
            // (El filtro global nos protege aquí también)
            var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.Id == dto.PacienteId);
            if (!pacienteExiste) return NotFound(new { mensaje = "Paciente no encontrado en esta clínica." });

            // 2. Armamos la cita uniendo las 3 piezas del rompecabezas
            var nuevaCita = new CitaMedica
            {
                PacienteId = dto.PacienteId,
                FechaHora = dto.FechaHora,
                MotivoConsulta = dto.MotivoConsulta,
                Estado = "Programada",
                ClinicaId = _context.CurrentClinicaId, // Extraído del Token (SaaS)
                MedicoId = _context.CurrentUserId      // Extraído del Token (Tú)
            };

            _context.CitaMedicas.Add(nuevaCita);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Cita agendada con éxito.", id = nuevaCita.Id });
        }
        // POST: api/Citas/{id}/completar
        [HttpPost("{id}/completar")]
        public async Task<IActionResult> CompletarCita(int id, [FromBody] CitaCompletarDto dto)
        {
            var cita = await _context.CitaMedicas.FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null) return NotFound(new { mensaje = "Cita no encontrada." });
            if (cita.Estado != "Programada") return BadRequest(new { mensaje = "La cita ya fue completada o cancelada." });

            cita.Estado = "Completada";

            var nuevoCobro = new Cobro
            {
                CitaMedicaId = cita.Id,
                ClinicaId = _context.CurrentClinicaId, // Blindaje SaaS
                Monto = dto.Monto,
                EstadoPago = "Pendiente"
            };

            _context.Cobros.Add(nuevoCobro);

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Cita completada. Pase del paciente a caja generado.", cobroId = nuevoCobro.Id });
        }
    }
}