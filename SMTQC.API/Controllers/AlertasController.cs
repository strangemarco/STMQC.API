using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMTQC.API.Models;

namespace SMTQC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertasController : ControllerBase
    {
        private readonly SmtqcDbContext _context;

        public AlertasController(SmtqcDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var alertas = await _context.Alertas
                .Include(a => a.FocoCalor)
                .Select(a => new
                {
                    a.Id,
                    a.Tipo,
                    a.Fecha,
                    a.Ubicacion,
                    a.Estado,
                    a.FocoCalorId,
                    FocoCalor = a.FocoCalor != null ? new 
                    {
                        a.FocoCalor.Id,
                        a.FocoCalor.Latitud,
                        a.FocoCalor.Longitud,
                        a.FocoCalor.Estado
                    } : null
                })
                .ToListAsync();

            return Ok(alertas);
        }

        [HttpPost("{id}/notificar")]
        public async Task<IActionResult> EnviarNotificacion(int id)
        {
            // Simular un retardo de red
            await Task.Delay(1500);
            
            // Simular respuesta con error de conexión (503 Service Unavailable o 504 Gateway Timeout)
            return StatusCode(503, new { message = "Error de conexión con el servidor de notificaciones externo. Tiempo de espera agotado." });
        }
    }
}
