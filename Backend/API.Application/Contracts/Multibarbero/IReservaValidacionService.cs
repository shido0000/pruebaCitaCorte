using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Application.Contracts.Multibarbero
{
    /// <summary>
    /// Contrato para el servicio de validación de reservas.
    /// Se encarga de verificar solapamientos, fechas válidas y disponibilidad de horarios.
    /// </summary>
    public interface IReservaValidacionService
    {
        /// <summary>
        /// Verifica si existe un solapamiento de horarios para un proveedor en un rango de fechas específico.
        /// </summary>
        /// <param name="proveedorId">ID del barbero o barbería.</param>
        /// <param name="fechaInicio">Fecha y hora de inicio de la nueva reserva.</param>
        /// <param name="fechaFin">Fecha y hora de fin de la nueva reserva.</param>
        /// <param name="reservaExcluirId">ID de una reserva a excluir del cálculo (útil para actualizaciones).</param>
        /// <returns>True si existe solapamiento, False si el horario está disponible.</returns>
        Task<bool> ExisteSolapamientoAsync(Guid proveedorId, DateTime fechaInicio, DateTime fechaFin, Guid? reservaExcluirId = null);

        /// <summary>
        /// Valida que las fechas de la reserva sean coherentes (inicio antes que fin, no en pasado, etc.).
        /// </summary>
        /// <param name="fechaInicio">Fecha y hora de inicio.</param>
        /// <param name="fechaFin">Fecha y hora de fin.</param>
        /// <returns>True si las fechas son válidas, False en caso contrario.</returns>
        Task<bool> ValidarFechasReservaAsync(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene una lista de horarios disponibles para un proveedor en una fecha específica.
        /// </summary>
        /// <param name="proveedorId">ID del proveedor.</param>
        /// <param name="fecha">Fecha para consultar disponibilidad.</param>
        /// <param name="duracionServicio">Duración del servicio en minutos.</param>
        /// <returns>Lista de fechas/horas disponibles.</returns>
        Task<List<DateTime>> ObtenerHorariosDisponiblesAsync(Guid proveedorId, DateTime fecha, int duracionServicio);

        /// <summary>
        /// Método estático auxiliar para determinar si dos rangos de tiempo se solapan.
        /// Fórmula: (InicioA < FinB) AND (FinA > InicioB)
        /// </summary>
        bool HaySolapamiento(DateTime inicio1, DateTime fin1, DateTime inicio2, DateTime fin2);
    }
}
