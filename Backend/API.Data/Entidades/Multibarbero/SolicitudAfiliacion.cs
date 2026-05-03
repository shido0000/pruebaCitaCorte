using API.Data.Enum.Multibarbero;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Solicitud de afiliación de un barbero a una barbería
    /// </summary>
    public class SolicitudAfiliacion : EntidadBase
    {
        [ForeignKey(nameof(Barbero))]
        public Guid BarberoId { get; set; }
        public PerfilBarbero Barbero { get; set; } = null!;

        [ForeignKey(nameof(Barberia))]
        public Guid BarberiaId { get; set; }
        public PerfilBarberia Barberia { get; set; } = null!;

        public EstadoSolicitudAfiliacion Estado { get; set; } = EstadoSolicitudAfiliacion.Pendiente;

        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string? MotivoRechazo { get; set; }
        public string? Notas { get; set; }

        [ForeignKey(nameof(RespondidoPor))]
        public Guid? RespondidoPorId { get; set; }
        public dynamic? RespondidoPor { get; set; } // Usuario que respondió (Admin/Comercial o Barbería)
    }
}
