using System.Text.Json.Serialization;

namespace API.Application.Dtos.Multibarbero.PlanSuscripcion
{
    public class CrearPlanSuscripcionInputDto : PlanSuscripcionDto
    {
        [JsonIgnore]
        public new Guid Id { get; set; }
        [JsonIgnore]
        public new bool Activo { get; set; }
    }
}
