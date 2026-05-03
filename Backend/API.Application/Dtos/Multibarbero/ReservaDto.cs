using API.Application.Dtos.Comunes;
using API.Data.Enum.Multibarbero;

namespace API.Application.Dtos.Multibarbero.Reserva
{
    public class ReservaDto : EntidadBaseDto
    {
        public required DateTime FechaHoraInicio { get; set; }
        public required DateTime FechaHoraFin { get; set; }
        public required EstadoReserva Estado { get; set; }
        public string? Notas { get; set; }
        public decimal PrecioTotal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid BarberoId { get; set; }
        public Guid ClienteId { get; set; }
        public Guid BarberiaId { get; set; }
        public Guid ServicioId { get; set; }
    }
}
