using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MediCore.Api.Models;
using MediCore.Api.DTOs;

namespace MediCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly MediCoreContext _context;

        public ReportesController(MediCoreContext context)
        {
            _context = context;
        }

        [HttpGet("ingresos-acumulados")]
        public async Task<IActionResult> ObtenerIngresosAcumulados()
        {
            // Extraemos el ID de tu clínica de la pulsera VIP para blindar el reporte
            int clinicaId = _context.CurrentClinicaId;

            // 🌟 MAGIA SQL: Window Functions desde C#
            // SUM(Monto) OVER (...) calcula el total acumulado fila por fila.
            var reporte = await _context.Database.SqlQuery<ReporteIngresoDto>(
                $@"SELECT 
                    Id AS CobroId, 
                    FechaEmision AS Fecha, 
                    Monto, 
                    SUM(Monto) OVER(PARTITION BY ClinicaId ORDER BY FechaEmision) AS AcumuladoHistorico
                   FROM Cobro 
                   WHERE EstadoPago = 'Pagado' AND ClinicaId = {clinicaId}"
            ).ToListAsync();

            return Ok(reporte);
        }
    }
}