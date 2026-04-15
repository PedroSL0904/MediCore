using MediCore.Api.DTOs;

namespace MediCore.Api.Services.Interfaces
{
    public interface ICitaService
    {
        Task<List<CitaResponseDto>> ObtenerCitasAsync();
        Task<int> AgendarCitaAsync(CitaRegistroDto dto);
        Task<(bool exito, int? cobroId)> CompletarCitaAsync(int citaId, decimal monto);
    }
}