using MediCore.Api.DTOs;

namespace MediCore.Api.Services.Interfaces
{
    public interface IPacienteService
    {
        Task<PaginacionResponseDto<PacienteResponseDto>> ObtenerPacientesAsync(int pagina, int recordsPorPagina);
        Task<int> RegistrarPacienteAsync(PacienteRegistroDto dto);
        Task<bool> ArchivarPacienteAsync(int id);
    }
}