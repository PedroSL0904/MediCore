using Microsoft.EntityFrameworkCore;
using MediCore.Api.Models;
using MediCore.Api.DTOs;
using MediCore.Api.Services.Interfaces;

namespace MediCore.Api.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly MediCoreContext _context;

        public PacienteService(MediCoreContext context)
        {
            _context = context;
        }

        public async Task<PaginacionResponseDto<PacienteResponseDto>> ObtenerPacientesAsync(int pagina, int recordsPorPagina)
        {
            var query = _context.Pacientes.AsQueryable();
            var totalRegistros = await query.CountAsync();
            var paginasTotales = (int)Math.Ceiling(totalRegistros / (double)recordsPorPagina);

            var pacientes = await query
                .AsNoTracking()
                .Skip((pagina - 1) * recordsPorPagina)
                .Take(recordsPorPagina)
                .Select(p => new PacienteResponseDto
                {
                    Id = p.Id,
                    NombreCompleto = p.NombreCompleto,
                    Telefono = p.Telefono
                })
                .ToListAsync();

            return new PaginacionResponseDto<PacienteResponseDto>
            {
                PaginaActual = pagina,
                PaginasTotales = paginasTotales,
                TotalRegistros = totalRegistros,
                Datos = pacientes
            };
        }

        public async Task<int> RegistrarPacienteAsync(PacienteRegistroDto dto)
        {
            var nuevoPaciente = new Paciente
            {
                NombreCompleto = dto.NombreCompleto,
                Telefono = dto.Telefono,

                ClinicaId = _context.CurrentClinicaId
            };

            _context.Pacientes.Add(nuevoPaciente);
            await _context.SaveChangesAsync();

            return nuevoPaciente.Id;
        }

        public async Task<bool> ArchivarPacienteAsync(int id)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == id);
            if (paciente == null) return false;

            paciente.Activo = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}