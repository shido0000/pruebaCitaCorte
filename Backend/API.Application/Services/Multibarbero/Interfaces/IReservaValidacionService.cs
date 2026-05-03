using API.Application.Dtos.Multibarbero;

namespace API.Application.Services.Multibarbero.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de validación de reservas
    /// </summary>
    public interface IReservaValidacionService
    {
        /// <summary>
        /// Verifica si existe solapamiento de horarios para un proveedor
        /// </summary>
        Task<bool> ExisteSolapamientoAsync(Guid proveedorId, DateTime fechaInicio, DateTime fechaFin, Guid? reservaExcluirId = null);
        
        /// <summary>
        /// Valida que las fechas de la reserva sean correctas
        /// </summary>
        Task<bool> ValidarFechasReservaAsync(DateTime fechaInicio, DateTime fechaFin);
        
        /// <summary>
        /// Obtiene los horarios disponibles para un proveedor en una fecha específica
        /// </summary>
        Task<List<DateTime>> ObtenerHorariosDisponiblesAsync(Guid proveedorId, DateTime fecha, int duracionServicio);
    }
}
