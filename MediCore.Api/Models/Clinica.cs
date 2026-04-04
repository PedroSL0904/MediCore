using System;
using System.Collections.Generic;

namespace MediCore.Api.Models;

public partial class Clinica
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Rfc { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<CitaMedica> CitaMedicas { get; set; } = new List<CitaMedica>();

    public virtual ICollection<Cobro> Cobros { get; set; } = new List<Cobro>();

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
