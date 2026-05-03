using System.ComponentModel.DataAnnotations.Schema;
using API.Data.Entidades.Multibarbero;
using API.Data.Enum.Multibarbero;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Reserva de servicio realizada por un cliente a un barbero o barbería
    /// </summary>
    public class Reserva : EntidadBase
    {
        [ForeignKey(nameof(Cliente))]
        public Guid ClienteId { get; set; }
        public PerfilCliente Cliente { get; set; } = null!;

        [ForeignKey(nameof(Proveedor))]
        public Guid ProveedorId { get; set; }
        public dynamic Proveedor { get; set; } = null!; // Puede ser PerfilBarbero o PerfilBarberia

        public TipoProveedor TipoProveedor { get; set; }

        [ForeignKey(nameof(Servicio))]
        public Guid ServicioId { get; set; }
        public Servicio Servicio { get; set; } = null!;

        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;

        public string? NotasCliente { get; set; }
        public string? NotasProveedor { get; set; }
        public string? MotivoRechazo { get; set; }

        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaConfirmacion { get; set; }
        public DateTime? FechaCancelacion { get; set; }

        public decimal? CalificacionCliente { get; set; }
        public string? ComentarioCliente { get; set; }

        public decimal PrecioTotal { get; set; }
        public string? MetodoPago { get; set; }
        public string? EstadoPago { get; set; }
    }
}
