using API.Application.Dtos.Multibarbero.Afiliaciones;

namespace API.Application.Contracts.Multibarbero
{
    public interface IAfiliacionService
    {
        // Gestión de Solicitudes de Afiliación
        Task<SolicitudAfiliacionDto> CrearSolicitudAfiliacionAsync(CrearAfiliacionDto input);
        Task<List<SolicitudAfiliacionDto>> ObtenerSolicitudesPendientesAsync(Guid barberiaId);
        Task ResponderSolicitudAfiliacionAsync(Guid solicitudId, bool aprobada, string? motivo = null);
        Task CancelarSolicitudAfiliacionAsync(Guid solicitudId);

        // Gestión de Afiliaciones Activas
        Task<AfiliacionDetallesDto> ObtenerAfiliacionActivaBarberoAsync(Guid barberoId);
        Task<List<AfiliacionDetallesDto>> ObtenerAfiliacionesActivasBarberiaAsync(Guid barberiaId);
        Task FinalizarAfiliacionAsync(Guid afiliacionId, string motivo);

        // Validaciones
        Task<bool> TieneAfiliacionActivaAsync(Guid barberoId);
        Task<bool> TieneCupoDisponibleAsync(Guid barberiaId);
        Task<int> ObtenerCupoOcupadoAsync(Guid barberiaId);

        // Redirección de reservas
        Task<Guid?> ObtenerBarberiaParaReservaAsync(Guid barberoId);
    }
}
