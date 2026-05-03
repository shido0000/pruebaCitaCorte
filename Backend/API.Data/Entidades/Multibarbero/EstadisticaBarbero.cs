using System.ComponentModel.DataAnnotations.Schema;
using API.Data.Entidades.Multibarbero;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Estadísticas mensuales/anuales de un barbero
    /// </summary>
    public class EstadisticaBarbero : EntidadBase
    {
        [ForeignKey(nameof(Barbero))]
        public Guid BarberoId { get; set; }
        public PerfilBarbero Barbero { get; set; } = null!;

        public int PeriodoMes { get; set; } // 1-12
        public int PeriodoAnio { get; set; }

        public int TotalReservas { get; set; }
        public int TotalClientesUnicos { get; set; }
        public decimal IngresosTotales { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public decimal TasaCancelacion { get; set; } // Porcentaje

        public string ServiciosMasSolicitadosJson { get; set; } = "[]"; // JSON con ranking de servicios
    }
}
