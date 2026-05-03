using API.Application.Dtos.Multibarbero.Suscripciones;

namespace API.Application.Contracts.Multibarbero
{
    public interface ISuscripcionService
    {
        // Gestión de Suscripciones
        Task<SuscripcionDetalleDto> ObtenerSuscripcionActualAsync(Guid usuarioId, API.Data.Enum.Multibarbero.TipoProveedor tipoProveedor);
        Task<List<SuscripcionDetalleDto>> ObtenerHistorialSuscripcionesAsync(Guid usuarioId, API.Data.Enum.Multibarbero.TipoProveedor tipoProveedor);

        // Cambio de Plan
        Task SolicitarCambioPlanAsync(SolicitarCambioPlanInputDto input);
        Task ResponderCambioPlanAsync(ResponderCambioPlanInputDto input);
        Task CancelarSolicitudCambioPlanAsync(Guid solicitudId);

        // Renovación
        Task RenovarSuscripcionAsync(Guid perfilId, API.Data.Enum.Multibarbero.TipoProveedor tipoProveedor);

        // Validaciones
        Task<bool> TieneSuscripcionActivaAsync(Guid perfilId, API.Data.Enum.Multibarbero.TipoProveedor tipoProveedor);
        Task<bool> PuedeAccederFuncionalidadAsync(Guid perfilId, string funcionalidad);

        // Reportes para Admin/Comercial
        Task<object> ObtenerReporteSuscripcionesPorVencerAsync(int dias = 7);
        Task<object> ObtenerReporteSuscripcionesActivasAsync();
    }
}
