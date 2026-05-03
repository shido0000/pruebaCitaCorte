using Hangfire;
using API.Domain.Interfaces.Multibarbero;
using Microsoft.Extensions.Logging;

namespace API.Domain.Jobs;

/// <summary>
/// Job programado para enviar recordatorios de reservas pendientes
/// Se ejecuta cada 30 minutos
/// </summary>
public class RecordatorioReservasPendientesJob
{
    private readonly IReservaService _reservaService;
    private readonly INotificacionService _notificacionService;
    private readonly ILogger<RecordatorioReservasPendientesJob> _logger;

    public RecordatorioReservasPendientesJob(
        IReservaService reservaService,
        INotificacionService notificacionService,
        ILogger<RecordatorioReservasPendientesJob> logger)
    {
        _reservaService = reservaService;
        _notificacionService = notificacionService;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task Ejecutar()
    {
        _logger.LogInformation("Iniciando job: RecordatorioReservasPendientes");
        
        try
        {
            // Obtener reservas pendientes por más de 1 hora
            var reservasPendientes = await _reservaService.ObtenerPendientesPorTiempo(TimeSpan.FromHours(1));
            
            foreach (var reserva in reservasPendientes)
            {
                // Notificar al proveedor (barbero o barbería)
                var usuarioProveedorId = reserva.ProveedorTipo == Data.Enum.Multibarbero.TipoProveedor.Barbero 
                    ? reserva.ProveedorBarbero.UsuarioId 
                    : reserva.ProveedorBarberia.UsuarioId;
                
                await _notificacionService.CrearNotificacion(
                    usuarioProveedorId,
                    Data.Enum.Multibarbero.TipoNotificacion.ReservaNueva,
                    "Tienes una reserva pendiente de confirmación",
                    $"La reserva del cliente {reserva.Cliente.Usuario.Nombre} para el servicio '{reserva.Servicio.Nombre}' lleva más de 1 hora pendiente. Por favor confirma o rechaza la reserva.",
                    entidadRelacionadaId: reserva.Id,
                    tipoEntidad: "Reserva",
                    accionRequerida: true,
                    urlAccion: $"/reservas/detalles/{reserva.Id}"
                );
                
                _logger.LogInformation($"Recordatorio enviado para reserva pendiente #{reserva.Id}");
            }
            
            _logger.LogInformation($"Job completado. Recordatorios enviados: {reservasPendientes.Count}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al ejecutar RecordatorioReservasPendientes");
            throw;
        }
    }
}
