using System;
using System.Collections.Generic;

namespace MediCore.Api.Models;

public partial class CitaMedica
{
    public int Id { get; set; }

    public int ClinicaId { get; set; }

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public DateTime FechaHora { get; set; }

    public string? MotivoConsulta { get; set; }

    public string? Estado { get; set; }

    public virtual Clinica Clinica { get; set; } = null!;

    public virtual ICollection<Cobro> Cobros { get; set; } = new List<Cobro>();

    public virtual Usuario Medico { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;
}
