using System.Text.Json.Serialization;

namespace API.Application.Dtos.Multibarbero.Notificacion
{
    public class CrearNotificacionInputDto : NotificacionDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
        [JsonIgnore]
        public new DateTime FechaCreacion { get; set; }
        [JsonIgnore]
        public new bool Leida { get; set; }
        [JsonIgnore]
        public new DateTime? FechaLeida { get; set; }
    }
}
