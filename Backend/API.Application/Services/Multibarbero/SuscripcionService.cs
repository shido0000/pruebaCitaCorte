using API.Application.Contracts.Multibarbero;
using API.Application.Dtos.Multibarbero.Suscripciones;
using API.Data.Enum.Multibarbero;

namespace API.Application.Services.Multibarbero
{
    public class SuscripcionService : ISuscripcionService
    {
        private readonly IPlanSuscripcionRepository _planRepository;
        private readonly IPerfilBarberoRepository _barberoRepository;
        private readonly IPerfilBarberiaRepository _barberiaRepository;
        private readonly ILimitesPlanRepository _limitesRepository;

        public SuscripcionService(
            IPlanSuscripcionRepository planRepository,
            IPerfilBarberoRepository barberoRepository,
            IPerfilBarberiaRepository barberiaRepository,
            ILimitesPlanRepository limitesRepository)
        {
            _planRepository = planRepository;
            _barberoRepository = barberoRepository;
            _barberiaRepository = barberiaRepository;
            _limitesRepository = limitesRepository;
        }

        public async Task<SuscripcionDetalleDto> ObtenerSuscripcionActualAsync(Guid usuarioId, TipoProveedor tipoProveedor)
        {
            if (tipoProveedor == TipoProveedor.Barbero)
            {
                var barbero = await _barberoRepository.ObtenerPorUsuarioIdAsync(usuarioId);
                if (barbero == null || !barbero.PlanSuscripcionId.HasValue)
                    return null!;

                var plan = await _planRepository.ObtenerPorIdAsync(barbero.PlanSuscripcionId.Value);
                if (plan == null)
                    return null!;

                return new SuscripcionDetalleDto
                {
                    PerfilId = barbero.Id,
                    TipoProveedor = TipoProveedor.Barbero,
                    PlanId = plan.Id,
                    NombrePlan = plan.Nombre,
                    TipoPlan = plan.TipoPlan,
                    Precio = plan.Precio,
                    FechaInicio = barbero.FechaInicioPlan,
                    FechaVencimiento = barbero.FechaVencimientoPlan,
                    Estado = barbero.EstadoSolicitudCambioPlan ?? EstadoSuscripcion.Activa,
                    Caracteristicas = plan.CaracteristicasJson,
                    DiasRestantes = barbero.FechaVencimientoPlan.HasValue
                        ? (barbero.FechaVencimientoPlan.Value - DateTime.UtcNow).Days
                        : 0
                };
            }
            else
            {
                var barberia = await _barberiaRepository.ObtenerPorUsuarioIdAsync(usuarioId);
                if (barberia == null || !barberia.PlanSuscripcionId.HasValue)
                    return null!;

                var plan = await _planRepository.ObtenerPorIdAsync(barberia.PlanSuscripcionId.Value);
                if (plan == null)
                    return null!;

                return new SuscripcionDetalleDto
                {
                    PerfilId = barberia.Id,
                    TipoProveedor = TipoProveedor.Barberia,
                    PlanId = plan.Id,
                    NombrePlan = plan.Nombre,
                    TipoPlan = plan.TipoPlan,
                    Precio = plan.Precio,
                    FechaInicio = barberia.FechaInicioPlan,
                    FechaVencimiento = barberia.FechaVencimientoPlan,
                    Estado = barberia.EstadoSolicitudCambioPlan ?? EstadoSuscripcion.Activa,
                    Caracteristicas = plan.CaracteristicasJson,
                    DiasRestantes = barberia.FechaVencimientoPlan.HasValue
                        ? (barberia.FechaVencimientoPlan.Value - DateTime.UtcNow).Days
                        : 0
                };
            }
        }

        public async Task<List<SuscripcionDetalleDto>> ObtenerHistorialSuscripcionesAsync(Guid usuarioId, TipoProveedor tipoProveedor)
        {
            // Implementación básica - se puede expandir para guardar historial completo
            var suscripcionActual = await ObtenerSuscripcionActualAsync(usuarioId, tipoProveedor);
            return suscripcionActual != null ? new List<SuscripcionDetalleDto> { suscripcionActual } : new List<SuscripcionDetalleDto>();
        }

        public async Task SolicitarCambioPlanAsync(SolicitarCambioPlanInputDto input)
        {
            var nuevoPlan = await _planRepository.ObtenerPorIdAsync(input.NuevoPlanId);
            if (nuevoPlan == null)
                throw new Exception("Plan no encontrado");

            if (input.TipoProveedor == TipoProveedor.Barbero)
            {
                var barbero = await _barberoRepository.ObtenerPorIdAsync(input.PerfilId);
                if (barbero == null)
                    throw new Exception("Barbero no encontrado");

                if (barbero.EstadoSolicitudCambioPlan.HasValue &&
                    barbero.EstadoSolicitudCambioPlan.Value == EstadoSuscripcion.PendienteCambio)
                    throw new Exception("Ya existe una solicitud de cambio de plan pendiente");

                barbero.NuevoPlanSolicitadoId = input.NuevoPlanId;
                barbero.EstadoSolicitudCambioPlan = EstadoSuscripcion.PendienteCambio;

                await _barberoRepository.ActualizarAsync(barbero);
            }
            else
            {
                var barberia = await _barberiaRepository.ObtenerPorIdAsync(input.PerfilId);
                if (barberia == null)
                    throw new Exception("Barbería no encontrada");

                if (barberia.EstadoSolicitudCambioPlan.HasValue &&
                    barberia.EstadoSolicitudCambioPlan.Value == EstadoSuscripcion.PendienteCambio)
                    throw new Exception("Ya existe una solicitud de cambio de plan pendiente");

                barberia.NuevoPlanSolicitadoId = input.NuevoPlanId;
                barberia.EstadoSolicitudCambioPlan = EstadoSuscripcion.PendienteCambio;

                await _barberiaRepository.ActualizarAsync(barberia);
            }
        }

        public async Task ResponderCambioPlanAsync(ResponderCambioPlanInputDto input)
        {
            if (input.TipoProveedor == TipoProveedor.Barbero)
            {
                var barbero = await _barberoRepository.ObtenerPorIdAsync(input.PerfilId);
                if (barbero == null)
                    throw new Exception("Barbero no encontrado");

                if (!barbero.EstadoSolicitudCambioPlan.HasValue ||
                    barbero.EstadoSolicitudCambioPlan.Value != EstadoSuscripcion.PendienteCambio)
                    throw new Exception("No hay una solicitud de cambio de plan pendiente");

                if (input.Aprobada && barbero.NuevoPlanSolicitadoId.HasValue)
                {
                    barbero.PlanSuscripcionId = barbero.NuevoPlanSolicitadoId;
                    barbero.FechaInicioPlan = DateTime.UtcNow;
                    barbero.FechaVencimientoPlan = DateTime.UtcNow.AddMonths(1); // Ajustar según duración del plan
                    barbero.NuevoPlanSolicitadoId = null;
                    barbero.EstadoSolicitudCambioPlan = EstadoSuscripcion.Activa;
                }
                else
                {
                    barbero.NuevoPlanSolicitadoId = null;
                    barbero.EstadoSolicitudCambioPlan = EstadoSuscripcion.Rechazada;
                }

                await _barberoRepository.ActualizarAsync(barbero);
            }
            else
            {
                var barberia = await _barberiaRepository.ObtenerPorIdAsync(input.PerfilId);
                if (barberia == null)
                    throw new Exception("Barbería no encontrada");

                if (!barberia.EstadoSolicitudCambioPlan.HasValue ||
                    barberia.EstadoSolicitudCambioPlan.Value != EstadoSuscripcion.PendienteCambio)
                    throw new Exception("No hay una solicitud de cambio de plan pendiente");

                if (input.Aprobada && barberia.NuevoPlanSolicitadoId.HasValue)
                {
                    barberia.PlanSuscripcionId = barberia.NuevoPlanSolicitadoId;
                    barberia.FechaInicioPlan = DateTime.UtcNow;
                    barberia.FechaVencimientoPlan = DateTime.UtcNow.AddMonths(1); // Ajustar según duración del plan
                    barberia.NuevoPlanSolicitadoId = null;
                    barberia.EstadoSolicitudCambioPlan = EstadoSuscripcion.Activa;
                }
                else
                {
                    barberia.NuevoPlanSolicitadoId = null;
                    barberia.EstadoSolicitudCambioPlan = EstadoSuscripcion.Rechazada;
                }

                await _barberiaRepository.ActualizarAsync(barberia);
            }
        }

        public async Task CancelarSolicitudCambioPlanAsync(Guid solicitudId)
        {
            // Esta implementación asume que solicitudId es el perfilId
            // Se puede ajustar para usar una entidad de solicitud separada si es necesario
            var barbero = await _barberoRepository.ObtenerPorIdAsync(solicitudId);
            if (barbero != null && barbero.EstadoSolicitudCambioPlan == EstadoSuscripcion.PendienteCambio)
            {
                barbero.NuevoPlanSolicitadoId = null;
                barbero.EstadoSolicitudCambioPlan = EstadoSuscripcion.Cancelada;
                await _barberoRepository.ActualizarAsync(barbero);
                return;
            }

            var barberia = await _barberiaRepository.ObtenerPorIdAsync(solicitudId);
            if (barberia != null && barberia.EstadoSolicitudCambioPlan == EstadoSuscripcion.PendienteCambio)
            {
                barberia.NuevoPlanSolicitadoId = null;
                barberia.EstadoSolicitudCambioPlan = EstadoSuscripcion.Cancelada;
                await _barberiaRepository.ActualizarAsync(barberia);
                return;
            }

            throw new Exception("Solicitud de cambio de plan no encontrada");
        }

        public async Task RenovarSuscripcionAsync(Guid perfilId, TipoProveedor tipoProveedor)
        {
            if (tipoProveedor == TipoProveedor.Barbero)
            {
                var barbero = await _barberoRepository.ObtenerPorIdAsync(perfilId);
                if (barbero == null || !barbero.PlanSuscripcionId.HasValue)
                    throw new Exception("Barbero sin suscripción activa");

                barbero.FechaVencimientoPlan = barbero.FechaVencimientoPlan?.AddMonths(1) ?? DateTime.UtcNow.AddMonths(1);
                await _barberoRepository.ActualizarAsync(barbero);
            }
            else
            {
                var barberia = await _barberiaRepository.ObtenerPorIdAsync(perfilId);
                if (barberia == null || !barberia.PlanSuscripcionId.HasValue)
                    throw new Exception("Barbería sin suscripción activa");

                barberia.FechaVencimientoPlan = barberia.FechaVencimientoPlan?.AddMonths(1) ?? DateTime.UtcNow.AddMonths(1);
                await _barberiaRepository.ActualizarAsync(barberia);
            }
        }

        public async Task<bool> TieneSuscripcionActivaAsync(Guid perfilId, TipoProveedor tipoProveedor)
        {
            if (tipoProveedor == TipoProveedor.Barbero)
            {
                var barbero = await _barberoRepository.ObtenerPorIdAsync(perfilId);
                return barbero != null &&
                       barbero.PlanSuscripcionId.HasValue &&
                       (!barbero.FechaVencimientoPlan.HasValue || barbero.FechaVencimientoPlan > DateTime.UtcNow);
            }
            else
            {
                var barberia = await _barberiaRepository.ObtenerPorIdAsync(perfilId);
                return barberia != null &&
                       barberia.PlanSuscripcionId.HasValue &&
                       (!barberia.FechaVencimientoPlan.HasValue || barberia.FechaVencimientoPlan > DateTime.UtcNow);
            }
        }

        public async Task<bool> PuedeAccederFuncionalidadAsync(Guid perfilId, string funcionalidad)
        {
            // Verificar límites del plan para la funcionalidad solicitada
            var barbero = await _barberoRepository.ObtenerPorIdAsync(perfilId);
            if (barbero == null || !barbero.PlanSuscripcionId.HasValue)
                return false;

            var limites = await _limitesRepository.ObtenerPorPlanIdAsync(barbero.PlanSuscripcionId.Value);
            if (limites == null)
                return false;

            return funcionalidad.ToLower() switch
            {
                "estadisticas" => limites.PermiteEstadisticas,
                "reservas" => limites.PermiteReservas,
                "productos" => limites.PermiteProductos,
                _ => true
            };
        }

        public async Task<object> ObtenerReporteSuscripcionesPorVencerAsync(int dias = 7)
        {
            var fechaLimite = DateTime.UtcNow.AddDays(dias);

            // Obtener barberías por vencer
            var barberiasPorVencer = await _barberiaRepository.ObtenerPorVencimientoAsync(fechaLimite);

            // Obtener barberos por vencer
            var barberosPorVencer = await _barberoRepository.ObtenerPorVencimientoAsync(fechaLimite);

            return new
            {
                BarberiasPorVencer = barberiasPorVencer.Select(b => new
                {
                    b.Id,
                    b.NombreComercial,
                    b.FechaVencimientoPlan,
                    DiasRestantes = (b.FechaVencimientoPlan - DateTime.UtcNow).Days
                }),
                BarberosPorVencer = barberosPorVencer.Select(b => new
                {
                    b.Id,
                    UsuarioNombre = b.Usuario.Nombre,
                    b.FechaVencimientoPlan,
                    DiasRestantes = (b.FechaVencimientoPlan - DateTime.UtcNow).Days
                }),
                TotalPorVencer = barberiasPorVencer.Count() + barberosPorVencer.Count()
            };
        }

        public async Task<object> ObtenerReporteSuscripcionesActivasAsync()
        {
            var barberiasActivas = await _barberiaRepository.ObtenerConSuscripcionActivaAsync();
            var barberosActivos = await _barberoRepository.ObtenerConSuscripcionActivaAsync();

            return new
            {
                TotalBarberiasActivas = barberiasActivas.Count(),
                TotalBarberosActivos = barberosActivos.Count(),
                IngresosMensualesEstimados = barberiasActivas.Sum(b => b.PlanSuscripcion?.Precio ?? 0) +
                                            barberosActivos.Sum(b => b.PlanSuscripcion?.Precio ?? 0)
            };
        }
    }
}
