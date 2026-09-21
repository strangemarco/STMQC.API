using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMTQC.API.DTOs;
using SMTQC.API.Models;

namespace SMTQC.API.Services
{
    public class ProductorService : IProductorService
    {
        private readonly SmtqcDbContext _context;
        private readonly IMapper _mapper;

        public ProductorService(SmtqcDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductorDto>> GetAllAsync()
        {
            var productores = await _context.Productores.ToListAsync();
            return _mapper.Map<IEnumerable<ProductorDto>>(productores);
        }

        public async Task<ProductorDto?> GetByIdAsync(int id)
        {
            var productor = await _context.Productores.FindAsync(id);
            if (productor == null) return null;
            return _mapper.Map<ProductorDto>(productor);
        }

        public async Task<ProductorDto> CreateAsync(ProductorDto productorDto)
        {
            var productor = _mapper.Map<Productore>(productorDto);
            _context.Productores.Add(productor);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductorDto>(productor);
        }

        public async Task<bool> UpdateAsync(int id, ProductorDto productorDto)
        {
            var productor = await _context.Productores.FindAsync(id);
            if (productor == null) return false;

            productor.Nombre = productorDto.Nombre;
            productor.Documento = productorDto.Documento;
            productor.Comunidad = productorDto.Comunidad;
            productor.Telefono = productorDto.Telefono;

            _context.Productores.Update(productor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var productor = await _context.Productores.FindAsync(id);
            if (productor == null) return false;

            _context.Productores.Remove(productor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
