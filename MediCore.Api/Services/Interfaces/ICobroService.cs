using MediCore.Api.DTOs;

namespace MediCore.Api.Services.Interfaces
{
    public interface ICobroService
    {
        Task<List<CobroPendienteDto>> ObtenerPendientesAsync();
        Task<bool> RegistrarPagoAsync(int cobroId);
    }
}