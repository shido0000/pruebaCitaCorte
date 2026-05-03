using API.Data.Enum.Multibarbero;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Plan de suscripción base para barberos y barberías
    /// </summary>
    public class PlanSuscripcion : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionDias { get; set; }
        public TipoPlan Tipo { get; set; }
        public bool Activo { get; set; } = true;
    }
}
