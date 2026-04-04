using System;
using System.Collections.Generic;

namespace MediCore.Api.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public int ClinicaId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public bool? Activo { get; set; }

    public virtual ICollection<CitaMedica> CitaMedicas { get; set; } = new List<CitaMedica>();

    public virtual Clinica Clinica { get; set; } = null!;
}
