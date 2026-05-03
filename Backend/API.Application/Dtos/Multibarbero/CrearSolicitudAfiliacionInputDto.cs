using System.Text.Json.Serialization;

namespace API.Application.Dtos.Multibarbero.Afiliacion
{
    public class CrearSolicitudAfiliacionInputDto : SolicitudAfiliacionDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
        [JsonIgnore]
        public new DateTime FechaSolicitud { get; set; }
        [JsonIgnore]
        public new EstadoSolicitudAfiliacion Estado { get; set; }
        [JsonIgnore]
        public new DateTime? FechaRespuesta { get; set; }
        [JsonIgnore]
        public new string? MotivoRechazo { get; set; }
    }
}
