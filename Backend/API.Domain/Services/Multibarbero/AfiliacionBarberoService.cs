using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Multibarbero;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Multibarbero
{
    public class AfiliacionBarberoService : BasicService<AfiliacionBarbero>, IAfiliacionBarberoService
    {
        private readonly IAfiliacionBarberoRepository _afiliacionRepository;

        public AfiliacionBarberoService(IUnitOfWork<AfiliacionBarbero> repositorios, IHttpContextAccessor httpContext)
            : base(repositorios, httpContext)
        {
            _afiliacionRepository = (IAfiliacionBarberoRepository)repositorios.BasicRepository;
        }

        public async Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarbero(Guid barberoId)
        {
            return await _afiliacionRepository.ObtenerPorBarberoAsync(barberoId);
        }

        public async Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarberia(Guid barberiaId)
        {
            return await _afiliacionRepository.ObtenerPorBarberiaAsync(barberiaId);
        }

        public async Task<AfiliacionBarbero?> ObtenerAfiliacionActiva(Guid barberoId)
        {
            return await _afiliacionRepository.ObtenerAfiliacionActivaPorBarberoAsync(barberoId);
        }

        public async Task<AfiliacionBarbero> CrearAfiliacion(AfiliacionBarbero afiliacion)
        {
            var existeActiva = await ObtenerAfiliacionActiva(afiliacion.BarberoId);
            if (existeActiva != null)
            {
                throw new CustomException
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "El barbero ya tiene una afiliación activa."
                };
            }

            await Crear(afiliacion);
            await SalvarCambios();
            return afiliacion;
        }

        public async Task DesactivarAfiliacion(Guid afiliacionId, string motivo)
        {
            var afiliacion = await ObtenerPorId(afiliacionId) ??
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Afiliación no encontrada." };

            afiliacion.Activo = false;
            afiliacion.FechaFinAfiliacion = DateTime.Now;
            afiliacion.MotivoFin = motivo;

            await Actualizar(afiliacion);
            await SalvarCambios();
        }
    }
}
