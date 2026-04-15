namespace MediCore.Api.DTOs
{
    public class CitaResponseDto
    {
        public int Id { get; set; }
        public string PacienteNombre { get; set; } = string.Empty; 
        public DateTime FechaHora { get; set; }
        public string? Motivo { get; set; } 
        public string Estado { get; set; } = string.Empty;
    }

    public class CobroPendienteDto
    {
        public int CobroId { get; set; }
        public string PacienteNombre { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }
}