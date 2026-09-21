using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMTQC.API.Models;
using SMTQC.API.DTOs;
using System.Linq;

namespace SMTQC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FocosCalorController : ControllerBase
    {
        private readonly SmtqcDbContext _context;

        public FocosCalorController(SmtqcDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            // Retornamos un objeto anónimo con GeometriaWkt para no complicar el MVP con otro DTO para Focos
            var focos = await _context.FocosCalors
                .Select(f => new 
                {
                    f.Id,
                    f.Fecha,
                    f.Hora,
                    f.Latitud,
                    f.Longitud,
                    GeometriaWkt = f.Ubicacion != null ? f.Ubicacion.ToText() : null,
                    f.Confianza,
                    f.Estado
                })
                .ToListAsync();

            return Ok(focos);
        }
    }
}
