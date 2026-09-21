using System.Threading.Tasks;

namespace SMTQC.API.Services
{
    public interface ITrazabilidadService
    {
        /// <summary>
        /// Procesa un Foco de Calor y determina su estado (Autorizado, Fuera de parámetro, No autorizado)
        /// y genera una alerta si corresponde.
        /// </summary>
        Task ProcesarFocoCalorAsync(int focoCalorId);
    }
}
