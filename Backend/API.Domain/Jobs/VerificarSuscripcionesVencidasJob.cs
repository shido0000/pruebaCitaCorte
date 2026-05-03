using Hangfire;
using API.Domain.Interfaces.Multibarbero;
using Microsoft.Extensions.Logging;

namespace API.Domain.Jobs;

/// <summary>
/// Job programado para verificar suscripciones vencidas
/// Se ejecuta cada hora
/// </summary>
public class VerificarSuscripcionesVencidasJob
{
    private readonly IPerfilBarberoService _perfilBarberoService;
    private readonly IPerfilBarberiaService _perfilBarberiaService;
    private readonly INotificacionService _notificacionService;
    private readonly ILogger<VerificarSuscripcionesVencidasJob> _logger;

    public VerificarSuscripcionesVencidasJob(
        IPerfilBarberoService perfilBarberoService,
        IPerfilBarberiaService perfilBarberiaService,
        INotificacionService notificacionService,
        ILogger<VerificarSuscripcionesVencidasJob> logger)
    {
        _perfilBarberoService = perfilBarberoService;
        _perfilBarberiaService = perfilBarberiaService;
        _notificacionService = notificacionService;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task Ejecutar()
    {
        _logger.LogInformation("Iniciando job: VerificarSuscripcionesVencidas");
        
        try
        {
            var fechaActual = DateTime.UtcNow;
            
            // Verificar barberos vencidos
            var barberosVencidos = await _perfilBarberoService.ObtenerVencidos(fechaActual);
            foreach (var barbero in barberosVencidos)
            {
                // Downgrade a Free
                await _perfilBarberoService.AplicarDowngradeAFree(barbero.Id);
                
                await _notificacionService.CrearNotificacion(
                    barbero.UsuarioId,
                    Data.Enum.Multibarbero.TipoNotificacion.SuscripcionVencida,
                    "Tu suscripción ha vencido",
                    $"Tu plan ({barbero.PlanSuscripcion.Nombre}) ha vencido. Tu cuenta ha sido cambiada al plan Free. Para recuperar tus beneficios, por favor renueva tu suscripción.",
                    entidadRelacionadaId: barbero.Id,
                    tipoEntidad: "PerfilBarbero"
                );
                _logger.LogInformation($"Suscripción vencida aplicada al barbero {barbero.UsuarioId}, downgrade a Free realizado");
            }
            
            // Verificar barberías vencidas
            var barberiasVencidas = await _perfilBarberiaService.ObtenerVencidos(fechaActual);
            foreach (var barberia in barberiasVencidas)
            {
                // Desactivar barbería
                await _perfilBarberiaService.DesactivarPorVencimiento(barberia.Id);
                
                await _notificacionService.CrearNotificacion(
                    barberia.UsuarioId,
                    Data.Enum.Multibarbero.TipoNotificacion.SuscripcionVencida,
                    "Tu suscripción ha vencido",
                    $"Tu plan ({barberia.PlanSuscripcion.Nombre}) ha vencido. Tu barbería ha sido desactivada temporalmente. Por favor renueva tu suscripción para volver a operar.",
                    entidadRelacionadaId: barberia.Id,
                    tipoEntidad: "PerfilBarberia"
                );
                _logger.LogInformation($"Suscripción vencida aplicada a la barbería {barberia.UsuarioId}, desactivación realizada");
            }
            
            _logger.LogInformation($"Job completado. Suscripciones vencidas procesadas: {barberosVencidos.Count + barberiasVencidas.Count}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar VerificarSuscripcionesVencidas");
            throw;
        }
    }
}
