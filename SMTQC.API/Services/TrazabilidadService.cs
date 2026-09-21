using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SMTQC.API.Models;

namespace SMTQC.API.Services
{
    public class TrazabilidadService : ITrazabilidadService
    {
        private readonly SmtqcDbContext _context;

        public TrazabilidadService(SmtqcDbContext context)
        {
            _context = context;
        }

        public async Task ProcesarFocoCalorAsync(int focoCalorId)
        {
            var foco = await _context.FocosCalors.FindAsync(focoCalorId);
            if (foco == null || foco.Ubicacion == null) return;

            // 1. Cruce Espacial: Buscar autorizaciones cuyo polígono contenga el punto del foco de calor
            // ST_Contains(Autorizacion.Geometria, Foco.Ubicacion)
            var autorizacionEspacial = await _context.Autorizaciones
                .Where(a => a.Estado == "Activo" && a.Geometria != null && a.Geometria.Contains(foco.Ubicacion))
                .FirstOrDefaultAsync();

            if (autorizacionEspacial != null)
            {
                // 2. Cruce Temporal
                // Convertir TimeOnly/DateOnly si se usaran, pero aquí Fecha es un Date o DateTime
                var fechaFoco = foco.Fecha;
                
                var fechaInicioAuth = DateOnly.FromDateTime(autorizacionEspacial.FechaInicio);
                var fechaFinAuth = DateOnly.FromDateTime(autorizacionEspacial.FechaFin);

                if (fechaFoco >= fechaInicioAuth && fechaFoco <= fechaFinAuth)
                {
                    foco.Estado = "Autorizado";
                }
                else
                {
                    foco.Estado = "Fuera de parámetro";
                    
                    // Generar Alerta
                    _context.Alertas.Add(new Alerta
                    {
                        Tipo = "Incumplimiento de Fechas",
                        Fecha = DateTime.UtcNow,
                        Ubicacion = $"Lat: {foco.Latitud}, Lon: {foco.Longitud}",
                        Estado = "No atendida",
                        FocoCalorId = foco.Id
                    });
                }
            }
            else
            {
                foco.Estado = "No autorizado";
                
                // Generar Alerta
                _context.Alertas.Add(new Alerta
                {
                    Tipo = "Quema Ilegal",
                    Fecha = DateTime.UtcNow,
                    Ubicacion = $"Lat: {foco.Latitud}, Lon: {foco.Longitud}",
                    Estado = "No atendida",
                    FocoCalorId = foco.Id
                });
            }

            _context.FocosCalors.Update(foco);
            await _context.SaveChangesAsync();
        }
    }
}
