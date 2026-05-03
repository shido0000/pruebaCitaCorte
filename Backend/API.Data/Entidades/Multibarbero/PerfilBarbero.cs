using System.Text.Json.Serialization;
using API.Data.Entidades;
using API.Data.Entidades.Multibarbero;
using API.Data.Entidades.Seguridad;
using API.Data.Enum.Multibarbero;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Perfil del barbero que extiende Usuario
    /// </summary>
    public class PerfilBarbero : EntidadBase
    {
        [ForeignKey(nameof(Usuario))]
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public string? Biografia { get; set; }
        public int ExperienciaAnios { get; set; }
        public string EspecialidadesJson { get; set; } = "[]"; // JSON array of especialidades
        public string? FotoPerfil { get; set; }
        public string? Ubicacion { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public int TotalReseñas { get; set; }
        
        // Campos de Suscripción (FALTANTES - AGREGADOS)
        public Guid? PlanSuscripcionId { get; set; }
        [ForeignKey(nameof(PlanSuscripcionId))]
        public PlanSuscripcion? PlanSuscripcion { get; set; }
        
        public DateTime? FechaInicioPlan { get; set; }
        public DateTime? FechaVencimientoPlan { get; set; }
        
        public EstadoSuscripcion? EstadoSolicitudCambioPlan { get; set; }
        public Guid? NuevoPlanSolicitadoId { get; set; }
        [ForeignKey(nameof(NuevoPlanSolicitadoId))]
        public PlanSuscripcion? NuevoPlanSolicitado { get; set; }
        
        // Estadísticas del barbero
        public int TotalServicios { get; set; }
        public int TotalClientes { get; set; }

        // Navegación a barbería si aplica
        public Guid? PerfilBarberiaId { get; set; }
        [ForeignKey(nameof(PerfilBarberiaId))]
        public PerfilBarberia? PerfilBarberia { get; set; }

        // Horarios (puede ser colección separada)
        // public ICollection<HorarioBarbero> Horarios { get; set; } = new List<HorarioBarbero>();
    }
}
