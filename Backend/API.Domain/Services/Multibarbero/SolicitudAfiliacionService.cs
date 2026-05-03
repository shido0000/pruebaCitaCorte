using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Multibarbero
{
    public class SolicitudAfiliacionService : BasicService<SolicitudAfiliacion, SolicitudAfiliacionValidator>, ISolicitudAfiliacionService
    {
        public SolicitudAfiliacionService(IUnitOfWork<SolicitudAfiliacion> repositorios, IHttpContextAccessor httpContext)
            : base(repositorios, httpContext)
        {
        }

        public async Task<IEnumerable<SolicitudAfiliacion>> ObtenerSolicitudesPorBarbero(Guid barberoId)
        {
            return await _repositorios.BasicRepository.GetQuery()
                .Where(s => s.BarberoId == barberoId)
                .Include(s => s.Barberia)
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<IEnumerable<SolicitudAfiliacion>> ObtenerSolicitudesPorBarberia(Guid barberiaId)
        {
            return await _repositorios.BasicRepository.GetQuery()
                .Where(s => s.BarberiaId == barberiaId)
                .Include(s => s.Barbero)
                .ThenInclude(b => b.Usuario)
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<SolicitudAfiliacion?> AprobarSolicitud(Guid solicitudId, Guid respondidoPorId)
        {
            var solicitud = await ObtenerPorId(solicitudId) ??
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Solicitud no encontrada." };

            if (solicitud.Estado != API.Data.Enum.Multibarbero.EstadoSolicitudAfiliacion.Pendiente)
            {
                throw new CustomException { Status = StatusCodes.Status400BadRequest, Message = "La solicitud ya ha sido procesada." };
            }

            solicitud.Estado = API.Data.Enum.Multibarbero.EstadoSolicitudAfiliacion.Aprobada;
            solicitud.FechaRespuesta = DateTime.Now;
            solicitud.RespondidoPorId = respondidoPorId;

            await Actualizar(solicitud);
            await SalvarCambios();

            return solicitud;
        }

        public async Task<SolicitudAfiliacion?> RechazarSolicitud(Guid solicitudId, string motivo, Guid respondidoPorId)
        {
            var solicitud = await ObtenerPorId(solicitudId) ??
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Solicitud no encontrada." };

            if (solicitud.Estado != API.Data.Enum.Multibarbero.EstadoSolicitudAfiliacion.Pendiente)
            {
                throw new CustomException { Status = StatusCodes.Status400BadRequest, Message = "La solicitud ya ha sido procesada." };
            }

            solicitud.Estado = API.Data.Enum.Multibarbero.EstadoSolicitudAfiliacion.Rechazada;
            solicitud.FechaRespuesta = DateTime.Now;
            solicitud.MotivoRechazo = motivo;
            solicitud.RespondidoPorId = respondidoPorId;

            await Actualizar(solicitud);
            await SalvarCambios();

            return solicitud;
        }
    }
}
