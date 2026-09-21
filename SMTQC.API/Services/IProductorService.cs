using System.Collections.Generic;
using System.Threading.Tasks;
using SMTQC.API.DTOs;

namespace SMTQC.API.Services
{
    public interface IProductorService
    {
        Task<IEnumerable<ProductorDto>> GetAllAsync();
        Task<ProductorDto?> GetByIdAsync(int id);
        Task<ProductorDto> CreateAsync(ProductorDto productorDto);
        Task<bool> UpdateAsync(int id, ProductorDto productorDto);
        Task<bool> DeleteAsync(int id);
    }
}
