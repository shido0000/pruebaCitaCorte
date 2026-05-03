using System.ComponentModel.DataAnnotations.Schema;
using API.Data.Entidades.Multibarbero;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Estadísticas mensuales/anuales de una barbería
    /// </summary>
    public class EstadisticaBarberia : EntidadBase
    {
        [ForeignKey(nameof(Barberia))]
        public Guid BarberiaId { get; set; }
        public PerfilBarberia Barberia { get; set; } = null!;

        public int PeriodoMes { get; set; } // 1-12
        public int PeriodoAnio { get; set; }

        public int TotalReservas { get; set; }
        public int TotalClientesUnicos { get; set; }
        public decimal IngresosTotales { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public decimal TasaOcupacion { get; set; } // Porcentaje

        public string BarberosMasActivosJson { get; set; } = "[]"; // JSON con ranking de barberos
        public string ServiciosMasSolicitadosJson { get; set; } = "[]"; // JSON con ranking de servicios
    }
}
