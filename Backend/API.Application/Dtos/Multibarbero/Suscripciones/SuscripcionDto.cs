namespace API.Application.Dtos.Multibarbero.Suscripciones
{
    /// <summary>
    /// DTO para solicitar cambio de plan de suscripción
    /// </summary>
    public class SolicitarCambioPlanInputDto
    {
        /// <summary>
        /// ID del perfil (Barbero o Barbería)
        /// </summary>
        public Guid PerfilId { get; set; }

        /// <summary>
        /// Tipo de perfil (Barbero o Barbería)
        /// </summary>
        public string TipoPerfil { get; set; } = string.Empty; // "Barbero" o "Barberia"

        /// <summary>
        /// ID del nuevo plan solicitado
        /// </summary>
        public Guid NuevoPlanId { get; set; }

        /// <summary>
        /// Notas o comentarios adicionales
        /// </summary>
        public string? Notas { get; set; }
    }

    /// <summary>
    /// DTO para aprobar/rechazar cambio de plan
    /// </summary>
    public class ResponderCambioPlanInputDto
    {
        /// <summary>
        /// ID del perfil cuyo plan se está modificando
        /// </summary>
        public Guid PerfilId { get; set; }

        /// <summary>
        /// Tipo de perfil (Barbero o Barbería)
        /// </summary>
        public string TipoPerfil { get; set; } = string.Empty;

        /// <summary>
        /// Acción a realizar (Aprobar/Rechazar)
        /// </summary>
        public bool Aprobar { get; set; }

        /// <summary>
        /// Motivo en caso de rechazo
        /// </summary>
        public string? MotivoRechazo { get; set; }
    }

    /// <summary>
    /// DTO con información completa de suscripción
    /// </summary>
    public class SuscripcionDetalleDto
    {
        public Guid PerfilId { get; set; }
        public string NombrePerfil { get; set; } = string.Empty;
        public string TipoPerfil { get; set; } = string.Empty;

        public Guid? PlanActualId { get; set; }
        public string? PlanActualNombre { get; set; }
        public DateTime? FechaInicioPlan { get; set; }
        public DateTime? FechaVencimientoPlan { get; set; }
        public EstadoSuscripcionDto? EstadoSolicitud { get; set; }

        public Guid? NuevoPlanSolicitadoId { get; set; }
        public string? NuevoPlanSolicitadoNombre { get; set; }
        public DateTime? FechaSolicitudCambio { get; set; }
    }

    /// <summary>
    /// Enum DTO para estado de suscripción
    /// </summary>
    public enum EstadoSuscripcionDto
    {
        Pendiente = 1,
        Activo = 2,
        Vencido = 3,
        Cancelado = 4
    }
}
