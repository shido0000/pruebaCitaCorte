using API.Data.Entidades.Multibarbero;
using API.Data.Enum.Multibarbero;
using Microsoft.Extensions.Logging;

namespace API.Application.Services.Multibarbero
{
    /// <summary>
    /// Servicio de validación de reservas - Implementa reglas de negocio para solapamiento de horarios
    /// </summary>
    public class ReservaValidacionService : IReservaValidacionService
    {
        private readonly ILogger<ReservaValidacionService> _logger;

        public ReservaValidacionService(ILogger<ReservaValidacionService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Verifica si existe solapamiento de horarios para un proveedor
        /// Regla: No debe existir solapamiento en fechas para el mismo proveedor
        /// </summary>
        public Task<bool> ExisteSolapamientoAsync(Guid proveedorId, DateTime fechaInicio, DateTime fechaFin, Guid? reservaExcluirId = null)
        {
            // Esta implementación valida la lógica de solapamiento
            // La implementación real consultaría la base de datos
            
            // Lógica de solapamiento:
            // Dos rangos [A, B] y [C, D] se solapan si: !(B <= C || A >= D)
            // Equivalentemente: A < D && B > C
            
            _logger.LogInformation("Validando solapamiento para proveedor {ProveedorId} desde {Inicio} hasta {Fin}", 
                proveedorId, fechaInicio, fechaFin);

            // En producción, esto consultaría la BD
            // return await _reservaRepository.ExisteSolapamientoAsync(proveedorId, fechaInicio, fechaFin, reservaExcluirId);
            
            return Task.FromResult(false); // Placeholder - implementar con repository
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
        public Task<List<DateTime>> ObtenerHorariosDisponiblesAsync(Guid proveedorId, DateTime fecha, int duracionServicio)
        {
            // Esta implementación devolvería los horarios disponibles
            // Considerando:
            // 1. Horario laboral del proveedor
            // 2. Reservas existentes
            // 3. Tiempo de preparación entre servicios
            
            var horariosDisponibles = new List<DateTime>();
            
            // Placeholder - implementar con repository y lógica de horarios
            return Task.FromResult(horariosDisponibles);
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
