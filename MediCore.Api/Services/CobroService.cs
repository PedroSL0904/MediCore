using Microsoft.EntityFrameworkCore;
using MediCore.Api.Models;
using MediCore.Api.DTOs;
using MediCore.Api.Services.Interfaces;

namespace MediCore.Api.Services
{
    public class CobroService : ICobroService
    {
        private readonly MediCoreContext _context;
        public CobroService(MediCoreContext context) => _context = context;

        public async Task<List<CobroPendienteDto>> ObtenerPendientesAsync()
        {
            return await _context.Cobros
                .AsNoTracking()
                // CORRECCIÓN: Para llegar al nombre del paciente, tenemos que pasar por la CitaMedica
                .Include(c => c.CitaMedica)
                .ThenInclude(cita => cita.Paciente)
                // CORRECCIÓN: Tu columna se llama EstadoPago
                .Where(c => c.EstadoPago == "Pendiente")
                .Select(c => new CobroPendienteDto
                {
                    CobroId = c.Id,
                    PacienteNombre = c.CitaMedica.Paciente.NombreCompleto,
                    Monto = c.Monto
                }).ToListAsync();
        }

        public async Task<bool> RegistrarPagoAsync(int cobroId)
        {
            var cobro = await _context.Cobros.FirstOrDefaultAsync(c => c.Id == cobroId);
            if (cobro == null) return false;

            // CORRECCIÓN: Quitamos SaldoPendiente (no existe) y usamos EstadoPago
            cobro.EstadoPago = "Pagado";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}