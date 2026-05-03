using System.ComponentModel.DataAnnotations.Schema;
using API.Data.Entidades.Multibarbero;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Estadísticas mensuales/anuales de un producto
    /// </summary>
    public class EstadisticaProducto : EntidadBase
    {
        [ForeignKey(nameof(Producto))]
        public Guid ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        public int PeriodoMes { get; set; } // 1-12
        public int PeriodoAnio { get; set; }

        public int UnidadesVendidas { get; set; }
        public decimal IngresosTotales { get; set; }
        public decimal? CalificacionPromedio { get; set; }
    }
}
