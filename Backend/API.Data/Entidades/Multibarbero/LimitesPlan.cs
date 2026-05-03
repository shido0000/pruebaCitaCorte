using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Límites asociados a un plan de suscripción
    /// Define las restricciones y capacidades según el tipo de plan
    /// </summary>
    public class LimitesPlan : EntidadBase
    {
        [ForeignKey(nameof(PlanSuscripcion))]
        public Guid PlanSuscripcionId { get; set; }
        public PlanSuscripcion PlanSuscripcion { get; set; } = null!;

        // Límites para Barberías
        /// <summary>
        /// Número máximo de barberos que pueden afiliarse a una barbería con este plan
        /// </summary>
        public int MaxBarberosAfiliados { get; set; }

        /// <summary>
        /// Número máximo de reservas que se pueden manejar mensualmente
        /// </summary>
        public int MaxReservasMensuales { get; set; }

        /// <summary>
        /// Número máximo de productos que se pueden tener en venta
        /// </summary>
        public int MaxProductosVenta { get; set; }

        // Características habilitadas
        /// <summary>
        /// Indica si el plan permite acceso a estadísticas
        /// </summary>
        public bool PermiteEstadisticas { get; set; }

        /// <summary>
        /// Indica si el plan permite recibir reservas
        /// </summary>
        public bool PermiteReservas { get; set; }

        /// <summary>
        /// Indica si el plan permite vender productos
        /// </summary>
        public bool PermiteProductos { get; set; }
    }
}
