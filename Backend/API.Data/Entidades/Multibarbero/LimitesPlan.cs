using System.ComponentModel.DataAnnotations.Schema;
using API.Data.Entidades;
using API.Data.Entidades.Multibarbero;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Límites asociados a un plan de suscripción
    /// </summary>
    public class LimitesPlan : EntidadBase
    {
        [ForeignKey(nameof(PlanSuscripcion))]
        public Guid PlanSuscripcionId { get; set; }
        public PlanSuscripcion PlanSuscripcion { get; set; } = null!;

        public string TipoLimite { get; set; } = string.Empty; // e.g., "ReservasMensuales", "BarberosMaximos"
        public int Valor { get; set; }
        
        // Propiedades de auditoría heredadas de EntidadBase
    }
}
