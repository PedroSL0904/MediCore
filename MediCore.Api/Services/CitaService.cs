using Microsoft.EntityFrameworkCore;
using MediCore.Api.Models;
using MediCore.Api.DTOs;
using MediCore.Api.Services.Interfaces;

namespace MediCore.Api.Services
{
    public class CitaService : ICitaService
    {
        private readonly MediCoreContext _context;

        public CitaService(MediCoreContext context) => _context = context;

        public async Task<List<CitaResponseDto>> ObtenerCitasAsync()
        {
            return await _context.CitaMedicas
                .AsNoTracking()
                .Include(c => c.Paciente)
                .Select(c => new CitaResponseDto
                {
                    Id = c.Id,
                    // Si por alguna razón el paciente no viene, pon "Desconocido"
                    PacienteNombre = c.Paciente != null ? c.Paciente.NombreCompleto : "Desconocido",
                    FechaHora = c.FechaHora,
                    // Si el motivo es nulo, pon "Sin especificar"
                    Motivo = c.MotivoConsulta ?? "Sin especificar",
                    // Si el estado es nulo, pon "Programada"
                    Estado = c.Estado ?? "Programada"
                }).ToListAsync();
        }

        public async Task<int> AgendarCitaAsync(CitaRegistroDto dto)
        {
            // CORRECCIÓN: Usamos el modelo CitaMedica y agregamos los IDs obligatorios
            var cita = new CitaMedica
            {
                PacienteId = dto.PacienteId,
                FechaHora = dto.FechaHora,
                MotivoConsulta = dto.MotivoConsulta,
                Estado = "Programada",
                ClinicaId = _context.CurrentClinicaId,
                MedicoId = _context.CurrentUserId // Asumimos que el usuario logueado es el médico
            };

            _context.CitaMedicas.Add(cita);
            await _context.SaveChangesAsync();
            return cita.Id;
        }

        public async Task<(bool exito, int? cobroId)> CompletarCitaAsync(int citaId, decimal monto)
        {
            var cita = await _context.CitaMedicas.FirstOrDefaultAsync(c => c.Id == citaId);
            if (cita == null || cita.Estado == "Completada") return (false, null);

            cita.Estado = "Completada";

            // CORRECCIÓN: Adaptado a las columnas exactas de tu tabla Cobro
            var cobro = new Cobro
            {
                CitaMedicaId = citaId,
                Monto = monto,
                EstadoPago = "Pendiente",
                FechaEmision = DateTime.Now,
                ClinicaId = _context.CurrentClinicaId
            };

            _context.Cobros.Add(cobro);
            await _context.SaveChangesAsync();

            return (true, cobro.Id);
        }
    }
}