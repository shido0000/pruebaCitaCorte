using API.Application.Dtos.Comunes;
using API.Data.Enum.Multibarbero;

namespace API.Application.Dtos.Multibarbero.Afiliacion
{
    public class SolicitudAfiliacionDto : EntidadBaseDto
    {
        public required string Motivo { get; set; }
        public required EstadoSolicitudAfiliacion Estado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string? MotivoRechazo { get; set; }
        public Guid BarberoId { get; set; }
        public Guid BarberiaId { get; set; }
    }
}
