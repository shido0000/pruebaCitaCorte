using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Multibarbero
{
    public class NotificacionService : BasicService<Notificacion, NotificacionValidator>, INotificacionService
    {
        public NotificacionService(IUnitOfWork<Notificacion> repositorios, IHttpContextAccessor httpContext)
            : base(repositorios, httpContext)
        {
        }

        public async Task<IEnumerable<Notificacion>> ObtenerNotificacionesPorUsuario(Guid usuarioId, bool? leidas = null)
        {
            IQueryable<Notificacion> query = _repositorios.BasicRepository.GetQuery()
                .Where(n => n.UsuarioDestinoId == usuarioId);

            if (leidas.HasValue)
            {
                query = query.Where(n => n.Leida == leidas.Value);
            }

            return await query
                .Include(n => n.UsuarioDestino)
                .OrderByDescending(n => n.FechaCreado)
                .ToListAsync();
        }

        public async Task MarcarComoLeida(Guid notificacionId)
        {
            var notificacion = await ObtenerPorId(notificacionId) ??
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Notificación no encontrada." };

            if (notificacion.Leida)
            {
                throw new CustomException { Status = StatusCodes.Status400BadRequest, Message = "La notificación ya está marcada como leída." };
            }

            notificacion.Leida = true;
            notificacion.FechaLectura = DateTime.Now;

            await Actualizar(notificacion);
            await SalvarCambios();
        }

        public async Task MarcarTodasComoLeidas(Guid usuarioId)
        {
            var notificaciones = await _repositorios.BasicRepository.GetQuery()
                .Where(n => n.UsuarioDestinoId == usuarioId && !n.Leida)
                .ToListAsync();

            if (!notificaciones.Any())
            {
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "No hay notificaciones sin leer para este usuario." };
            }

            foreach (var notificacion in notificaciones)
            {
                notificacion.Leida = true;
                notificacion.FechaLectura = DateTime.Now;
            }

            await SalvarCambios();
        }

        public async Task<Notificacion> CrearNotificacion(Notificacion notificacion)
        {
            notificacion.FechaCreado = DateTime.Now;
            notificacion.Leida = false;

            await Crear(notificacion);
            await SalvarCambios();

            return notificacion;
        }
    }
}
