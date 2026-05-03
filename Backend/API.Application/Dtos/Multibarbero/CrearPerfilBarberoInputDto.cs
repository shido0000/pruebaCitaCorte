using System.Text.Json.Serialization;

namespace API.Application.Dtos.Multibarbero.PerfilBarbero
{
    public class CrearPerfilBarberoInputDto : PerfilBarberoDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
        [JsonIgnore]
        public new bool Activo { get; set; }
        [JsonIgnore]
        public new DateTime FechaRegistro { get; set; }
    }
}
