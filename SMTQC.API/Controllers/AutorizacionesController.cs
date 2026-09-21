using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMTQC.API.DTOs;
using SMTQC.API.Services;

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
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AutorizacionDto>>> GetAll()
        {
            var autorizaciones = await _autorizacionService.GetAllAsync();
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
            // For MVP efficiency, assign to default producer if not provided, and generate code
            if (dto.ProductorId <= 0) dto.ProductorId = 1;
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
    }
}
