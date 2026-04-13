namespace MediCore.Api.DTOs
{
    public class CitaRegistroDto
    {
        public int PacienteId { get; set; }
        public DateTime FechaHora { get; set; }
        public string MotivoConsulta { get; set; } = null!;
    }
}