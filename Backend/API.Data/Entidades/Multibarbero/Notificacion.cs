using API.Data.Entidades.Seguridad;
using API.Data.Enum.Multibarbero;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Notificación del sistema para usuarios
    /// </summary>
    public class Notificacion : EntidadBase
    {
        [ForeignKey(nameof(UsuarioDestino))]
        public Guid UsuarioDestinoId { get; set; }
        public Usuario UsuarioDestino { get; set; } = null!;

        public TipoNotificacion Tipo { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;

        public bool Leida { get; set; } = false;
        public DateTime? FechaLectura { get; set; }

        public Guid? EntidadRelacionadaId { get; set; }
        public string? TipoEntidad { get; set; } // Nombre de la entidad relacionada (Reserva, SolicitudAfiliacion, etc.)

        public bool AccionRequerida { get; set; } = false;
        public string? UrlAccion { get; set; }
    }
}
