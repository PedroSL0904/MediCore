using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCore.Api.Models;

namespace MediCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicasController : ControllerBase
    {
        private readonly MediCoreContext _context;

        // Aquí inyectamos el contexto de base de datos que acabas de configurar
        public ClinicasController(MediCoreContext context)
        {
            _context = context;
        }

        // GET: api/Clinicas (Para ver todas las clínicas registradas)
        [HttpGet]
        public async Task<IActionResult> ObtenerClinicas()
        {
            // Usamos Set<Clinica>() para evitar problemas si Visual Studio le cambió el nombre
            var clinicas = await _context.Set<Clinica>().ToListAsync();
            return Ok(clinicas);
        }

        // POST: api/Clinicas (Para guardar una clínica nueva)
        [HttpPost]
        public async Task<IActionResult> RegistrarClinica([FromBody] Clinica nuevaClinica)
        {
            nuevaClinica.FechaRegistro = DateTime.Now;
            _context.Set<Clinica>().Add(nuevaClinica);
            await _context.SaveChangesAsync(); // Esto ejecuta el comando INSERT en SQL Server

            return Ok(nuevaClinica);
        }
    }
}