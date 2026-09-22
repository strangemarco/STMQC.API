using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using SMTQC.API.Models;
using SMTQC.API.DTOs;

namespace SMTQC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductorController : ControllerBase
    {
        private readonly SmtqcDbContext _context;

        public ProductorController(SmtqcDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<ActionResult<ProductorDto>> GetMyProfile()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Usuario no identificado.");
            }

            var productor = await _context.Productores.FirstOrDefaultAsync(p => p.UsuarioId == userId);

            if (productor == null)
            {
                return NotFound("Perfil de productor no encontrado.");
            }

            return Ok(new ProductorDto
            {
                Id = productor.Id,
                Nombre = productor.Nombre,
                Documento = productor.Documento,
                Comunidad = productor.Comunidad,
                Telefono = productor.Telefono
            });
        }

        [HttpPost("me")]
        public async Task<ActionResult<ProductorDto>> CreateOrUpdateProfile(CreateUpdateProductorDto dto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Usuario no identificado.");
            }

            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
            {
                return NotFound("Usuario no existe en el sistema.");
            }

            var productor = await _context.Productores.FirstOrDefaultAsync(p => p.UsuarioId == userId);

            if (productor == null)
            {
                // Crear
                productor = new Productore
                {
                    UsuarioId = userId,
                    Nombre = usuario.Nombre, // Tomamos el nombre de la cuenta
                    Documento = dto.Documento,
                    Comunidad = dto.Comunidad,
                    Telefono = dto.Telefono
                };
                _context.Productores.Add(productor);
            }
            else
            {
                // Actualizar
                productor.Documento = dto.Documento;
                productor.Comunidad = dto.Comunidad;
                productor.Telefono = dto.Telefono;
            }

            await _context.SaveChangesAsync();

            return Ok(new ProductorDto
            {
                Id = productor.Id,
                Nombre = productor.Nombre,
                Documento = productor.Documento,
                Comunidad = productor.Comunidad,
                Telefono = productor.Telefono
            });
        }
    }
}
