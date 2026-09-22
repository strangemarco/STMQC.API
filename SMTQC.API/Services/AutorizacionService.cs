using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMTQC.API.DTOs;
using SMTQC.API.Models;

namespace SMTQC.API.Services
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly SmtqcDbContext _context;
        private readonly IMapper _mapper;

        public AutorizacionService(SmtqcDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AutorizacionDto>> GetAllAsync()
        {
            var autorizaciones = await _context.Autorizaciones
                .Include(a => a.Productor)
                .ToListAsync();
            return _mapper.Map<IEnumerable<AutorizacionDto>>(autorizaciones);
        }

        public async Task<AutorizacionDto?> GetByIdAsync(int id)
        {
            var autorizacion = await _context.Autorizaciones
                .Include(a => a.Productor)
                .FirstOrDefaultAsync(a => a.Id == id);
                
            if (autorizacion == null) return null;
            return _mapper.Map<AutorizacionDto>(autorizacion);
        }

        public async Task<AutorizacionDto> CreateAsync(AutorizacionCreateDto dto)
        {
            var autorizacion = _mapper.Map<Autorizacione>(dto);
            _context.Autorizaciones.Add(autorizacion);
            await _context.SaveChangesAsync();
            return _mapper.Map<AutorizacionDto>(autorizacion);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var autorizacion = await _context.Autorizaciones.FindAsync(id);
            if (autorizacion == null) return false;

            _context.Autorizaciones.Remove(autorizacion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeEstadoAsync(int id, string nuevoEstado)
        {
            var autorizacion = await _context.Autorizaciones.FindAsync(id);
            if (autorizacion == null) return false;

            autorizacion.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
