using API.Data.Entidades.Multibarbero;
using API.Domain.Validators.Multibarbero;

namespace API.Domain.Interfaces.Multibarbero
{
    public interface INotificacionService : IBaseService<Notificacion, NotificacionValidator>
    {
        Task<IEnumerable<Notificacion>> ObtenerNotificacionesPorUsuario(Guid usuarioId, bool? leidas = null);
        Task MarcarComoLeida(Guid notificacionId);
        Task MarcarTodasComoLeidas(Guid usuarioId);
        Task<Notificacion> CrearNotificacion(Notificacion notificacion);
    }
}
