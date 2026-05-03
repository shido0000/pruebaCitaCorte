using API.Domain.Interfaces.Multibarbero;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace API.Domain.Jobs;

/// <summary>
/// Job programado para verificar suscripciones por vencer (7 días antes)
/// Se ejecuta diariamente a las 8:00 AM
/// </summary>
public class VerificarSuscripcionesPorVencerJob
{
    private readonly IPerfilBarberoService _perfilBarberoService;
    private readonly IPerfilBarberiaService _perfilBarberiaService;
    private readonly INotificacionService _notificacionService;
    private readonly ILogger<VerificarSuscripcionesPorVencerJob> _logger;

    public VerificarSuscripcionesPorVencerJob(
        IPerfilBarberoService perfilBarberoService,
        IPerfilBarberiaService perfilBarberiaService,
        INotificacionService notificacionService,
        ILogger<VerificarSuscripcionesPorVencerJob> logger)
    {
        _perfilBarberoService = perfilBarberoService;
        _perfilBarberiaService = perfilBarberiaService;
        _notificacionService = notificacionService;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task Ejecutar()
    {
        _logger.LogInformation("Iniciando job: VerificarSuscripcionesPorVencer");

        try
        {
            var fechaVencimiento = DateTime.UtcNow.AddDays(7);

            // Verificar barberos
            var barberosPorVencer = await _perfilBarberoService.ObtenerPorFechaVencimiento(fechaVencimiento);
            foreach (var barbero in barberosPorVencer)
            {
                await _notificacionService.CrearNotificacion(
                    barbero.UsuarioId,
                    Data.Enum.Multibarbero.TipoNotificacion.SuscripcionPorVencer,
                    "Tu suscripción está por vencer",
                    $"Tu plan actual ({barbero.PlanSuscripcion.Nombre}) vencerá en 7 días. Por favor renueva tu suscripción para continuar disfrutando de todos los beneficios.",
                    entidadRelacionadaId: barbero.Id,
                    tipoEntidad: "PerfilBarbero"
                );
                _logger.LogInformation($"Notificación de suscripción por vencer enviada al barbero {barbero.UsuarioId}");
            }

            // Verificar barberías
            var barberiasPorVencer = await _perfilBarberiaService.ObtenerPorFechaVencimiento(fechaVencimiento);
            foreach (var barberia in barberiasPorVencer)
            {
                await _notificacionService.CrearNotificacion(
                    barberia.UsuarioId,
                    Data.Enum.Multibarbero.TipoNotificacion.SuscripcionPorVencer,
                    "Tu suscripción está por vencer",
                    $"Tu plan actual ({barberia.PlanSuscripcion.Nombre}) vencerá en 7 días. Por favor renueva tu suscripción para continuar operando normalmente.",
                    entidadRelacionadaId: barberia.Id,
                    tipoEntidad: "PerfilBarberia"
                );
                _logger.LogInformation($"Notificación de suscripción por vencer enviada a la barbería {barberia.UsuarioId}");
            }

            _logger.LogInformation($"Job completado. Notificaciones enviadas: {barberosPorVencer.Count + barberiasPorVencer.Count}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar VerificarSuscripcionesPorVencer");
            throw;
        }
    }
}
