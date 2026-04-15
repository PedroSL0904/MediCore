using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediCore.Api.DTOs;
using MediCore.Api.Services.Interfaces;

namespace MediCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPacientes([FromQuery] int pagina = 1, [FromQuery] int recordsPorPagina = 10)
        {
            if (recordsPorPagina > 50) recordsPorPagina = 50;
            if (pagina < 1) pagina = 1;

            var response = await _pacienteService.ObtenerPacientesAsync(pagina, recordsPorPagina);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPaciente([FromBody] PacienteRegistroDto dto)
        {
            var nuevoId = await _pacienteService.RegistrarPacienteAsync(dto);
            return Ok(new { mensaje = "Paciente registrado exitosamente.", id = nuevoId });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EliminarPaciente(int id)
        {
            var exito = await _pacienteService.ArchivarPacienteAsync(id);
            if (!exito) return NotFound(new { mensaje = "Paciente no encontrado." });

            return Ok(new { mensaje = "Expediente del paciente archivado correctamente." });
        }
    }
}