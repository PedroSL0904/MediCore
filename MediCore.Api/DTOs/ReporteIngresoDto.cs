namespace MediCore.Api.DTOs
{
    public class ReporteIngresoDto
    {
        public int CobroId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public decimal AcumuladoHistorico { get; set; }
    }
}