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
        [ForeignKey(nameof(Usuario))]
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        
        public string NombreComercial { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public string? FotoPortada { get; set; }
        public string? Descripcion { get; set; }
        
        // Coordenadas geográficas
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        
        // Horarios de atención
        public TimeSpan? HorarioApertura { get; set; }
        public TimeSpan? HorarioCierre { get; set; }
        public string DiasLaborablesJson { get; set; } = "[]"; // JSON array de días laborables
        
        // Capacidad y límites
        public int? CapacidadMaxima { get; set; }
        public int MaxBarberosPermitidos { get; set; }
        
        public decimal CalificacionPromedio { get; set; }
        public int TotalReseñas { get; set; }
        
        // Suscripción
        public Guid? PlanSuscripcionId { get; set; }
        [ForeignKey(nameof(PlanSuscripcionId))]
        public PlanSuscripcion? PlanSuscripcion { get; set; }
        
        public DateTime? FechaInicioPlan { get; set; }
        public DateTime? FechaVencimientoPlan { get; set; }
        
        public EstadoSuscripcion? EstadoSolicitudCambioPlan { get; set; }
        public Guid? NuevoPlanSolicitadoId { get; set; }
        [ForeignKey(nameof(NuevoPlanSolicitadoId))]
        public PlanSuscripcion? NuevoPlanSolicitado { get; set; }

        // Colección de barberos afiliados
        public ICollection<AfiliacionBarbero> AfiliacionesBarberos { get; set; } = new List<AfiliacionBarbero>();
        public ICollection<SolicitudAfiliacion> SolicitudesAfiliacion { get; set; } = new List<SolicitudAfiliacion>();
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}

