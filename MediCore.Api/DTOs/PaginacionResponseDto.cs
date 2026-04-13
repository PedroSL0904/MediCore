namespace MediCore.Api.DTOs
{
    // La <T> significa que es Genérico. Puede contener listas de cualquier DTO.
    public class PaginacionResponseDto<T>
    {
        public int PaginaActual { get; set; }
        public int PaginasTotales { get; set; }
        public int TotalRegistros { get; set; }
        public List<T> Datos { get; set; } = new List<T>();
    }
}