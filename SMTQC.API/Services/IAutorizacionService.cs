using System.Collections.Generic;
using System.Threading.Tasks;
using SMTQC.API.DTOs;

namespace SMTQC.API.Services
{
    public interface IAutorizacionService
    {
        Task<IEnumerable<AutorizacionDto>> GetAllAsync();
        Task<AutorizacionDto?> GetByIdAsync(int id);
        Task<AutorizacionDto> CreateAsync(AutorizacionCreateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangeEstadoAsync(int id, string nuevoEstado);
    }
}
