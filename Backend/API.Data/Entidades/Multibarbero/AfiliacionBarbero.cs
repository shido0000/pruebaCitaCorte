using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Afiliación activa de un barbero a una barbería
    /// </summary>
    public class AfiliacionBarbero : EntidadBase
    {
        [ForeignKey(nameof(Barbero))]
        public Guid BarberoId { get; set; }
        public PerfilBarbero Barbero { get; set; } = null!;

        [ForeignKey(nameof(Barberia))]
        public Guid BarberiaId { get; set; }
        public PerfilBarberia Barberia { get; set; } = null!;

        public DateTime FechaAfiliacion { get; set; }
        public bool Activo { get; set; } = true;
        public bool EsPrincipal { get; set; } // Indica si las reservas van a la barbería

        public DateTime? FechaFinAfiliacion { get; set; }
        public string? MotivoFin { get; set; }
    }
}
