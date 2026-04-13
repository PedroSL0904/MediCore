using System;
using System.Collections.Generic;

namespace MediCore.Api.Models;

public partial class Paciente
{
    public int Id { get; set; }

    public int ClinicaId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string? Telefono { get; set; }

    public bool Activo { get; set; } = true;

    public virtual ICollection<CitaMedica> CitaMedicas { get; set; } = new List<CitaMedica>();

    public virtual Clinica Clinica { get; set; } = null!;
}
