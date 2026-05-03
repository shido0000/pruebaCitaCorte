using API.Application.Dtos.Comunes;
using API.Data.Enum.Multibarbero;

namespace API.Application.Dtos.Multibarbero.Notificacion
{
    public class NotificacionDto : EntidadBaseDto
    {
        public required string Titulo { get; set; }
        public required string Mensaje { get; set; }
        public required TipoNotificacion Tipo { get; set; }
        public bool Leida { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaLeida { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
