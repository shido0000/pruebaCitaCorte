using API.Data.Entidades.Multibarbero;
using API.Domain.Validators.Multibarbero;

namespace API.Domain.Interfaces.Multibarbero
{
    public interface ISolicitudAfiliacionService : IBaseService<SolicitudAfiliacion, SolicitudAfiliacionValidator>
    {
        Task<IEnumerable<SolicitudAfiliacion>> ObtenerSolicitudesPorBarbero(Guid barberoId);
        Task<IEnumerable<SolicitudAfiliacion>> ObtenerSolicitudesPorBarberia(Guid barberiaId);
        Task<SolicitudAfiliacion?> AprobarSolicitud(Guid solicitudId, Guid respondidoPorId);
        Task<SolicitudAfiliacion?> RechazarSolicitud(Guid solicitudId, string motivo, Guid respondidoPorId);
    }
}
