using Hangfire;
using Microsoft.Extensions.Logging;

namespace API.Domain.Jobs;

/// <summary>
/// Job programado para calcular estadísticas mensuales
/// Se ejecuta el primero de cada mes
/// </summary>
public class CalcularEstadisticasMensualesJob
{
    private readonly IEstadisticaService _estadisticaService;
    private readonly ILogger<CalcularEstadisticasMensualesJob> _logger;

    public CalcularEstadisticasMensualesJob(
        IEstadisticaService estadisticaService,
        ILogger<CalcularEstadisticasMensualesJob> logger)
    {
        _estadisticaService = estadisticaService;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task Ejecutar()
    {
        _logger.LogInformation("Iniciando job: CalcularEstadisticasMensuales");

        try
        {
            var fechaActual = DateTime.UtcNow;
            var mesAnterior = fechaActual.AddMonths(-1);

            // Calcular estadísticas de barberos
            await _estadisticaService.CalcularEstadisticasBarberos(mesAnterior.Year, mesAnterior.Month);
            _logger.LogInformation("Estadísticas de barberos calculadas");

            // Calcular estadísticas de barberías
            await _estadisticaService.CalcularEstadisticasBarberias(mesAnterior.Year, mesAnterior.Month);
            _logger.LogInformation("Estadísticas de barberías calculadas");

            // Calcular estadísticas de productos
            await _estadisticaService.CalcularEstadisticasProductos(mesAnterior.Year, mesAnterior.Month);
            _logger.LogInformation("Estadísticas de productos calculadas");

            _logger.LogInformation($"Job completado. Estadísticas del mes {mesAnterior:yyyy-MM} generadas exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar CalcularEstadisticasMensuales");
            throw;
        }
    }
}
