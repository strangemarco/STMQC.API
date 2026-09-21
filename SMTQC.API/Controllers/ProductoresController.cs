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
    public class ProductoresController : ControllerBase
    {
        private readonly IProductorService _productorService;

        public ProductoresController(IProductorService productorService)
        {
            _productorService = productorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductorDto>>> GetAll()
        {
            var productores = await _productorService.GetAllAsync();
            return Ok(productores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductorDto>> GetById(int id)
        {
            var productor = await _productorService.GetByIdAsync(id);
            if (productor == null) return NotFound();
            return Ok(productor);
        }

        [HttpPost]
        public async Task<ActionResult<ProductorDto>> Create(ProductorDto productorDto)
        {
            var created = await _productorService.CreateAsync(productorDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductorDto productorDto)
        {
            if (id != productorDto.Id) return BadRequest();
            
            var success = await _productorService.UpdateAsync(id, productorDto);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _productorService.DeleteAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
