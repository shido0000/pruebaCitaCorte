using System.Text.Json.Serialization;

namespace API.Application.Dtos.Multibarbero.Reserva
{
    public class CrearReservaInputDto : ReservaDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
        [JsonIgnore]
        public new DateTime FechaCreacion { get; set; }
        [JsonIgnore]
        public new EstadoReserva Estado { get; set; }
    }
}
