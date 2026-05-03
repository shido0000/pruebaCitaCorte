using API.Application.Contracts.Multibarbero;
using API.Application.Dtos.Multibarbero.Afiliaciones;
using API.Data.Entidades.Multibarbero;
using API.Data.Enum.Multibarbero;
using API.Domain.Interfaces.Repositories.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services.Multibarbero
{
    public class AfiliacionService : IAfiliacionService
    {
        private readonly ISolicitudAfiliacionRepository _solicitudRepository;
        private readonly IAfiliacionBarberoRepository _afiliacionRepository;
        private readonly IPerfilBarberiaRepository _barberiaRepository;
        private readonly IPerfilBarberoRepository _barberoRepository;

        public AfiliacionService(
            ISolicitudAfiliacionRepository solicitudRepository,
            IAfiliacionBarberoRepository afiliacionRepository,
            IPerfilBarberiaRepository barberiaRepository,
            IPerfilBarberoRepository barberoRepository)
        {
            _solicitudRepository = solicitudRepository;
            _afiliacionRepository = afiliacionRepository;
            _barberiaRepository = barberiaRepository;
            _barberoRepository = barberoRepository;
        }

        public async Task<SolicitudAfiliacionDto> CrearSolicitudAfiliacionAsync(CrearAfiliacionDto input)
        {
            // Validar que el barbero no tenga otra afiliación activa
            if (await TieneAfiliacionActivaAsync(input.BarberoId))
                throw new Exception("El barbero ya tiene una afiliación activa");

            // Validar cupo disponible en la barbería
            if (!await TieneCupoDisponibleAsync(input.BarberiaId))
                throw new Exception("La barbería ha alcanzado su capacidad máxima de barberos");

            // Verificar si ya existe una solicitud pendiente
            var solicitudExistente = await _solicitudRepository.ExisteSolicitudPendienteAsync(input.BarberoId, input.BarberiaId);
            if (solicitudExistente)
                throw new Exception("Ya existe una solicitud de afiliación pendiente para este barbero y barbería");

            var solicitud = new SolicitudAfiliacion
            {
                BarberoId = input.BarberoId,
                BarberiaId = input.BarberiaId,
                FechaSolicitud = DateTime.UtcNow,
                Estado = EstadoSolicitudAfiliacion.Pendiente,
                MensajeBarbero = input.Mensaje
            };

            await _solicitudRepository.CrearAsync(solicitud);
            
            return new SolicitudAfiliacionDto
            {
                Id = solicitud.Id,
                BarberoId = solicitud.BarberoId,
                BarberiaId = solicitud.BarberiaId,
                FechaSolicitud = solicitud.FechaSolicitud,
                Estado = solicitud.Estado,
                MensajeBarbero = solicitud.MensajeBarbero
            };
        }

        public async Task<List<SolicitudAfiliacionDto>> ObtenerSolicitudesPendientesAsync(Guid barberiaId)
        {
            var solicitudes = await _solicitudRepository.ObtenerPendientesPorBarberiaAsync(barberiaId);
            return solicitudes.Select(s => new SolicitudAfiliacionDto
            {
                Id = s.Id,
                BarberoId = s.BarberoId,
                BarberiaId = s.BarberiaId,
                FechaSolicitud = s.FechaSolicitud,
                Estado = s.Estado,
                MensajeBarbero = s.MensajeBarbero,
                RespuestaBarberia = s.RespuestaBarberia,
                MotivoRespuesta = s.MotivoRespuesta
            }).ToList();
        }

        public async Task ResponderSolicitudAfiliacionAsync(Guid solicitudId, bool aprobada, string? motivo = null)
        {
            var solicitud = await _solicitudRepository.ObtenerPorIdAsync(solicitudId);
            if (solicitud == null)
                throw new Exception("Solicitud no encontrada");

            if (solicitud.Estado != EstadoSolicitudAfiliacion.Pendiente)
                throw new Exception("La solicitud ya ha sido respondida");

            solicitud.Estado = aprobada ? EstadoSolicitudAfiliacion.Aprobada : EstadoSolicitudAfiliacion.Rechazada;
            solicitud.RespuestaBarberia = DateTime.UtcNow;
            solicitud.MotivoRespuesta = motivo;

            if (aprobada)
            {
                // Crear la afiliación activa
                var afiliacion = new AfiliacionBarbero
                {
                    BarberoId = solicitud.BarberoId,
                    BarberiaId = solicitud.BarberiaId,
                    FechaInicio = DateTime.UtcNow,
                    Activo = true,
                    EsPrincipal = false // Se puede ajustar según lógica de negocio
                };

                await _afiliacionRepository.CrearAsync(afiliacion);
            }

            await _solicitudRepository.ActualizarAsync(solicitud);
        }

        public async Task CancelarSolicitudAfiliacionAsync(Guid solicitudId)
        {
            var solicitud = await _solicitudRepository.ObtenerPorIdAsync(solicitudId);
            if (solicitud == null)
                throw new Exception("Solicitud no encontrada");

            if (solicitud.Estado != EstadoSolicitudAfiliacion.Pendiente)
                throw new Exception("Solo se pueden cancelar solicitudes pendientes");

            solicitud.Estado = EstadoSolicitudAfiliacion.Cancelada;
            await _solicitudRepository.ActualizarAsync(solicitud);
        }

        public async Task<AfiliacionDetallesDto> ObtenerAfiliacionActivaBarberoAsync(Guid barberoId)
        {
            var afiliacion = await _afiliacionRepository.ObtenerActivaPorBarberoAsync(barberoId);
            if (afiliacion == null)
                return null!;

            return MapearAfiliacionDetalle(afiliacion);
        }

        public async Task<List<AfiliacionDetallesDto>> ObtenerAfiliacionesActivasBarberiaAsync(Guid barberiaId)
        {
            var afiliaciones = await _afiliacionRepository.ObtenerActivasPorBarberiaAsync(barberiaId);
            return afiliaciones.Select(MapearAfiliacionDetalle).ToList();
        }

        public async Task FinalizarAfiliacionAsync(Guid afiliacionId, string motivo)
        {
            var afiliacion = await _afiliacionRepository.ObtenerPorIdAsync(afiliacionId);
            if (afiliacion == null)
                throw new Exception("Afiliación no encontrada");

            if (!afiliacion.Activo)
                throw new Exception("La afiliación ya está inactiva");

            afiliacion.Activo = false;
            afiliacion.FechaFin = DateTime.UtcNow;
            afiliacion.MotivoFin = motivo;

            await _afiliacionRepository.ActualizarAsync(afiliacion);
        }

        public async Task<bool> TieneAfiliacionActivaAsync(Guid barberoId)
        {
            return await _afiliacionRepository.ExisteAfiliacionActivaAsync(barberoId);
        }

        public async Task<bool> TieneCupoDisponibleAsync(Guid barberiaId)
        {
            var barberia = await _barberiaRepository.ObtenerPorIdAsync(barberiaId);
            if (barberia == null)
                throw new Exception("Barbería no encontrada");

            var cupoOcupado = await ObtenerCupoOcupadoAsync(barberiaId);
            return cupoOcupado < (barberia.CapacidadMaxima ?? int.MaxValue);
        }

        public async Task<int> ObtenerCupoOcupadoAsync(Guid barberiaId)
        {
            return await _afiliacionRepository.ObtenerCupoOcupadoAsync(barberiaId);
        }

        public async Task<Guid?> ObtenerBarberiaParaReservaAsync(Guid barberoId)
        {
            var afiliacionPrincipal = await _afiliacionRepository.ObtenerAfiliacionPrincipalAsync(barberoId);
            if (afiliacionPrincipal != null && afiliacionPrincipal.Activo)
                return afiliacionPrincipal.BarberiaId;

            // Si no hay principal, retornar la primera activa encontrada
            var afiliaciones = await _afiliacionRepository.ObtenerActivasPorBarberoAsync(barberoId);
            return afiliaciones.FirstOrDefault()?.BarberiaId;
        }

        private AfiliacionDetallesDto MapearAfiliacionDetalle(AfiliacionBarbero afiliacion)
        {
            return new AfiliacionDetallesDto
            {
                Id = afiliacion.Id,
                BarberoId = afiliacion.BarberoId,
                BarberiaId = afiliacion.BarberiaId,
                FechaInicio = afiliacion.FechaInicio,
                FechaFin = afiliacion.FechaFin,
                Activo = afiliacion.Activo,
                EsPrincipal = afiliacion.EsPrincipal,
                MotivoFin = afiliacion.MotivoFin
            };
        }
    }
}
