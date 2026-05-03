using API.Application.Contracts.Multibarbero;
using API.Data.Enum.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Application.Services.Multibarbero
{
    /// <summary>
    /// Servicio de validación de reservas - Implementa reglas de negocio para solapamiento de horarios
    /// </summary>
    public class ReservaValidacionService : IReservaValidacionService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ILogger<ReservaValidacionService> _logger;

        public ReservaValidacionService(
            IReservaRepository reservaRepository,
            ILogger<ReservaValidacionService> logger)
        {
            _reservaRepository = reservaRepository;
            _logger = logger;
        }

        /// <summary>
        /// Verifica si existe solapamiento de horarios para un proveedor
        /// Regla: No debe existir solapamiento en fechas para el mismo proveedor
        /// </summary>
        public async Task<bool> ExisteSolapamientoAsync(Guid proveedorId, DateTime fechaInicio, DateTime fechaFin, Guid? reservaExcluirId = null)
        {
            _logger.LogInformation("Validando solapamiento para proveedor {ProveedorId} desde {Inicio} hasta {Fin}",
                proveedorId, fechaInicio, fechaFin);

            // Obtener todas las reservas activas del proveedor en el rango de fechas
            var reservas = await _reservaRepository.ObtenerTodosAsync();

            // Filtrar reservas del mismo proveedor que estén activas y no sean la que se está excluyendo
            var reservasConflicto = reservas.Where(r =>
                r.ProveedorId == proveedorId &&
                r.Estado != EstadoReserva.Cancelada &&
                r.Estado != EstadoReserva.Finalizada &&
                (reservaExcluirId == null || r.Id != reservaExcluirId) &&
                HaySolapamiento(fechaInicio, fechaFin, r.FechaInicio, r.FechaFin)
            ).ToList();

            if (reservasConflicto.Any())
            {
                _logger.LogWarning("Se encontraron {Count} reservas con solapamiento para el proveedor {ProveedorId}",
                    reservasConflicto.Count, proveedorId);
                return true;
            }

            _logger.LogInformation("No hay solapamiento para el proveedor {ProveedorId}", proveedorId);
            return false;
        }

        /// <summary>
        /// Valida que las fechas de la reserva sean correctas
        /// </summary>
        public Task<bool> ValidarFechasReservaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            // Validaciones básicas
            if (fechaInicio >= fechaFin)
            {
                _logger.LogWarning("Fecha de inicio {Inicio} es mayor o igual a fecha fin {Fin}", fechaInicio, fechaFin);
                return Task.FromResult(false);
            }

            if (fechaInicio < DateTime.Now)
            {
                _logger.LogWarning("Fecha de inicio {Inicio} es en el pasado", fechaInicio);
                return Task.FromResult(false);
            }

            // Validar que no sea demasiado lejos en el futuro (ej. 6 meses)
            if (fechaInicio > DateTime.Now.AddMonths(6))
            {
                _logger.LogWarning("Fecha de inicio {Inicio} es demasiado lejana", fechaInicio);
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Obtiene los horarios disponibles para un proveedor en una fecha específica
        /// </summary>
        public async Task<List<DateTime>> ObtenerHorariosDisponiblesAsync(Guid proveedorId, DateTime fecha, int duracionServicio)
        {
            _logger.LogInformation("Obteniendo horarios disponibles para proveedor {ProveedorId} en fecha {Fecha}",
                proveedorId, fecha.Date);

            // Obtener todas las reservas del proveedor para esa fecha
            var reservas = await _reservaRepository.ObtenerTodosAsync();
            var reservasDelDia = reservas.Where(r =>
                r.ProveedorId == proveedorId &&
                r.FechaInicio.Date == fecha.Date &&
                r.Estado != EstadoReserva.Cancelada &&
                r.Estado != EstadoReserva.Finalizada
            ).ToList();

            // Definir horario laboral (ej: 9:00 AM a 7:00 PM)
            var horaInicioJornada = fecha.Date.AddHours(9); // 9:00 AM
            var horaFinJornada = fecha.Date.AddHours(19);   // 7:00 PM

            var horariosDisponibles = new List<DateTime>();
            var tiempoActual = horaInicioJornada;

            // Generar slots de tiempo disponibles
            while (tiempoActual.AddMinutes(duracionServicio) <= horaFinJornada)
            {
                var finSlot = tiempoActual.AddMinutes(duracionServicio);

                // Verificar si este slot se solapa con alguna reserva existente
                bool hayConflicto = reservasDelDia.Any(r =>
                    HaySolapamiento(tiempoActual, finSlot, r.FechaInicio, r.FechaFin)
                );

                if (!hayConflicto)
                {
                    horariosDisponibles.Add(tiempoActual);
                }

                // Avanzar al siguiente slot (incrementos de 15 minutos)
                tiempoActual = tiempoActual.AddMinutes(15);
            }

            _logger.LogInformation("Se encontraron {Count} horarios disponibles", horariosDisponibles.Count);
            return horariosDisponibles;
        }

        /// <summary>
        /// Método auxiliar para verificar solapamiento entre dos rangos de tiempo
        /// Fórmula: (inicio1 < fin2) && (fin1 > inicio2)
        /// </summary>
        public static bool HaySolapamiento(DateTime inicio1, DateTime fin1, DateTime inicio2, DateTime fin2)
        {
            return (inicio1 < fin2) && (fin1 > inicio2);
        }
    }
}
