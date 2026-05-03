using API.Application.Dtos.Comunes;
using API.Data.Enum.Multibarbero;

namespace API.Application.Dtos.Multibarbero.PlanSuscripcion
{
    public class PlanSuscripcionDto : EntidadBaseDto
    {
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public required decimal PrecioMensual { get; set; }
        public required decimal PrecioAnual { get; set; }
        public required int DuracionDias { get; set; }
        public required TipoPlan TipoPlan { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
