using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using SMTQC.API.Models;
using SMTQC.API.Services;

namespace SMTQC.API.BackgroundServices
{
    public class NasaFirmsBackgroundService : BackgroundService
    {
        private readonly ILogger<NasaFirmsBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public NasaFirmsBackgroundService(ILogger<NasaFirmsBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Simulador de NASA FIRMS iniciado.");

            // Esperar un poco antes de empezar
            await Task.Delay(5000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Simulando obtención de focos de calor desde NASA FIRMS...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<SmtqcDbContext>();
                    var trazabilidadService = scope.ServiceProvider.GetRequiredService<ITrazabilidadService>();

                    // Generar un foco de calor aleatorio en Santa Cruz (cerca de -63.0, -17.7)
                    var rnd = new Random();
                    double lat = -17.75 + (rnd.NextDouble() * 0.1); // Rango aprox -17.75 a -17.65
                    double lon = -63.15 + (rnd.NextDouble() * 0.2); // Rango aprox -63.15 a -62.95

                    var foco = new FocosCalor
                    {
                        Fecha = DateOnly.FromDateTime(DateTime.UtcNow),
                        Hora = TimeOnly.FromDateTime(DateTime.UtcNow),
                        Latitud = lat,
                        Longitud = lon,
                        Ubicacion = new Point(lon, lat) { SRID = 4326 }, // SRID 4326 es el estándar GPS WGS84
                        Sensor = "MODIS",
                        Confianza = Math.Round(rnd.NextDouble() * 100, 2),
                        Estado = "Pendiente"
                    };

                    context.FocosCalors.Add(foco);
                    await context.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation($"Nuevo Foco de Calor detectado: Lat {lat}, Lon {lon}. Procesando trazabilidad...");

                    // 2. Procesar el foco para ver si es legal o ilegal
                    await trazabilidadService.ProcesarFocoCalorAsync(foco.Id);
                }

                // Esperar 30 segundos para la próxima simulación
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
