namespace MediCore.Api.DTOs
{
    public class UsuarioRegistroDto
    {
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Password { get; set; } = null!; 
        public int ClinicaId { get; set; }
    }
}