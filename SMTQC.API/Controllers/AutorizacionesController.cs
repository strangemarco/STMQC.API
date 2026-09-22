using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMTQC.API.DTOs;
using SMTQC.API.Services;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;

namespace SMTQC.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacionesController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;

        public AutorizacionesController(IAutorizacionService autorizacionService)
        {
            _autorizacionService = autorizacionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorizacionDto>>> GetAll()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var autorizaciones = await _autorizacionService.GetAllAsync();

            if (userRole == "Productor")
            {
                if (int.TryParse(userIdString, out int userId))
                {
                    // Necesitamos inyectar SmtqcDbContext o agregar GetByUsuarioId en el servicio, 
                    // lo filtraremos aquí para simplificar el MVP
                    var db = HttpContext.RequestServices.GetService(typeof(SMTQC.API.Models.SmtqcDbContext)) as SMTQC.API.Models.SmtqcDbContext;
                    var productor = await db!.Productores.FirstOrDefaultAsync(p => p.UsuarioId == userId);
                    if (productor != null)
                    {
                        autorizaciones = autorizaciones.Where(a => a.ProductorId == productor.Id).ToList();
                    }
                    else
                    {
                        return Ok(new List<AutorizacionDto>()); // No tiene perfil = no tiene solicitudes
                    }
                }
            }

            return Ok(autorizaciones);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AutorizacionDto>> GetById(int id)
        {
            var autorizacion = await _autorizacionService.GetByIdAsync(id);
            if (autorizacion == null) return NotFound();
            return Ok(autorizacion);
        }

        [HttpPost]
        public async Task<ActionResult<AutorizacionDto>> Create(AutorizacionCreateDto dto)
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                var db = HttpContext.RequestServices.GetService(typeof(SMTQC.API.Models.SmtqcDbContext)) as SMTQC.API.Models.SmtqcDbContext;
                var productor = await db!.Productores.FirstOrDefaultAsync(p => p.UsuarioId == userId);
                if (productor == null)
                {
                    return BadRequest("Debe completar su perfil de Productor antes de crear una solicitud.");
                }
                dto.ProductorId = productor.Id;
            }
            else
            {
                return Unauthorized("Usuario no identificado.");
            }

            if (string.IsNullOrEmpty(dto.Codigo)) dto.Codigo = $"Q-{DateTime.Now.Year}-{new Random().Next(1000, 9999)}";

            var created = await _autorizacionService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _autorizacionService.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/estado")]
        [Authorize(Roles = "Funcionario,Admin")]
        public async Task<IActionResult> ChangeEstado(int id, [FromBody] string nuevoEstado)
        {
            if (string.IsNullOrEmpty(nuevoEstado)) return BadRequest("Estado inválido");

            var success = await _autorizacionService.ChangeEstadoAsync(id, nuevoEstado);
            if (!success) return NotFound();

            return Ok();
        }
    }
}
