using System.ComponentModel.DataAnnotations.Schema;
using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Data.Entidades.Multibarbero;
using API.Data.Enum.Multibarbero;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Perfil de la barbería / salón
    /// </summary>
    public class PerfilBarberia : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public string? Descripcion { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public int TotalReseñas { get; set; }
        public TipoProveedor Tipo { get; set; }
        public Guid? PlanSuscripcionId { get; set; }
        [ForeignKey(nameof(PlanSuscripcionId))]
        public PlanSuscripcion? PlanSuscripcion { get; set; }

        // Colección de barberos afiliados
        public ICollection<PerfilBarbero> PerfilesBarberos { get; set; } = new List<PerfilBarbero>();
    }
}

