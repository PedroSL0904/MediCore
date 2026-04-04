using System;
using System.Collections.Generic;

namespace MediCore.Api.Models;

public partial class Cobro
{
    public int Id { get; set; }

    public int ClinicaId { get; set; }

    public int CitaMedicaId { get; set; }

    public decimal Monto { get; set; }

    public string? EstadoPago { get; set; }

    public DateTime? FechaEmision { get; set; }

    public virtual CitaMedica CitaMedica { get; set; } = null!;

    public virtual Clinica Clinica { get; set; } = null!;
}
