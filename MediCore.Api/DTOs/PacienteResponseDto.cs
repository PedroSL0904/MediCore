namespace MediCore.Api.DTOs
{
    public class PacienteResponseDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string? Telefono { get; set; }
    }
}