using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MediCore.Api.Models;

namespace MediCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CobrosController : ControllerBase
    {
        private readonly MediCoreContext _context;

        public CobrosController(MediCoreContext context)
        {
            _context = context;
        }

        // GET: api/Cobros/pendientes
        [HttpGet("pendientes")]
        public async Task<IActionResult> ObtenerCuentasPorCobrar()
        {
            // Cruzamos 3 tablas (Cobro -> Cita -> Paciente) para darle el nombre real a la recepcionista
            var cobrosPendientes = await _context.Cobros
                .Include(c => c.CitaMedica)
                    .ThenInclude(cm => cm.Paciente)
                .Where(c => c.EstadoPago == "Pendiente")
                .Select(c => new
                {
                    Id = c.Id,
                    Paciente = c.CitaMedica.Paciente.NombreCompleto,
                    Monto = c.Monto,
                    FechaEmision = c.FechaEmision
                })
                .ToListAsync();

            return Ok(cobrosPendientes);
        }

        // POST: api/Cobros/{id}/pagar
        [HttpPost("{id}/pagar")]
        public async Task<IActionResult> RegistrarPago(int id)
        {
            var cobro = await _context.Cobros.FirstOrDefaultAsync(c => c.Id == id);

            if (cobro == null) return NotFound(new { mensaje = "Cobro no encontrado." });
            if (cobro.EstadoPago == "Pagado") return BadRequest(new { mensaje = "Este recibo ya fue pagado." });

            cobro.EstadoPago = "Pagado";
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Pago registrado exitosamente. La cuenta está en ceros." });
        }
    }
}