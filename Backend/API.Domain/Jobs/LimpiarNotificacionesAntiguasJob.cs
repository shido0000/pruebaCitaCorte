using API.Data.Entidades.Multibarbero;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Domain.Jobs;

/// <summary>
/// Job programado para limpiar notificaciones antiguas (>30 días)
/// Se ejecuta semanalmente
/// </summary>
public class LimpiarNotificacionesAntiguasJob
{
    private readonly DbContext _dbContext;
    private readonly ILogger<LimpiarNotificacionesAntiguasJob> _logger;

    public LimpiarNotificacionesAntiguasJob(
        DbContext dbContext,
        ILogger<LimpiarNotificacionesAntiguasJob> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task Ejecutar()
    {
        _logger.LogInformation("Iniciando job: LimpiarNotificacionesAntiguas");

        try
        {
            var fechaCorte = DateTime.UtcNow.AddDays(-30);

            // Eliminar notificaciones antiguas
            var notificacionesAntiguas = await _dbContext.Set<Notificacion>()
                .Where(n => n.FechaCreacion < fechaCorte)
                .ToListAsync();

            if (notificacionesAntiguas.Any())
            {
                _dbContext.Set<Notificacion>().RemoveRange(notificacionesAntiguas);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Notificaciones antiguas eliminadas: {notificacionesAntiguas.Count}");
            }
            else
            {
                _logger.LogInformation("No hay notificaciones antiguas para eliminar");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar LimpiarNotificacionesAntiguas");
            throw;
        }
    }
}
