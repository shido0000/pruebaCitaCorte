using API.Data.Entidades;
using API.Data.Entidades.Multibarbero;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Característica de un plan de suscripción
    /// </summary>
    public class CaracteristicaPlan : EntidadBase
    {
        public Guid PlanSuscripcionId { get; set; }
        public PlanSuscripcion? PlanSuscripcion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
