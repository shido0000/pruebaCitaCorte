namespace API.Application.Dtos.Multibarbero.Suscripciones
{
    /// <summary>
    /// DTO con detalles completos de una suscripción
    /// </summary>
    public class SuscripcionDetalleDto
    {
        public Guid Id { get; set; }
        public Guid PerfilId { get; set; }
        public string TipoPerfil { get; set; } = string.Empty; // "Barbero" o "Barberia"
        public string NombrePerfil { get; set; } = string.Empty;
        
        // Plan actual
        public Guid? PlanSuscripcionId { get; set; }
        public string? PlanSuscripcionNombre { get; set; }
        public decimal PrecioPlan { get; set; }
        public int DuracionDias { get; set; }
        
        // Fechas del plan
        public DateTime? FechaInicioPlan { get; set; }
        public DateTime? FechaVencimientoPlan { get; set; }
        public int DiasRestantes => FechaVencimientoPlan.HasValue 
            ? (int)(FechaVencimientoPlan.Value - DateTime.UtcNow).TotalDays 
            : 0;
        
        // Estado de la suscripción
        public string Estado { get; set; } = string.Empty; // Activo, PorVencer, Vencido, Pendiente
        
        // Solicitud de cambio pendiente
        public bool TieneSolicitudPendiente { get; set; }
        public Guid? SolicitudCambioPlanId { get; set; }
        public Guid? NuevoPlanSolicitadoId { get; set; }
        public string? NuevoPlanSolicitadoNombre { get; set; }
        public DateTime? FechaSolicitudCambio { get; set; }
        public string? EstadoSolicitudCambio { get; set; }
        
        // Límites y características del plan
        public int MaxBarberosAfiliados { get; set; }
        public int MaxReservasMensuales { get; set; }
        public int MaxProductosVenta { get; set; }
        public bool PermiteEstadisticas { get; set; }
        public bool PermiteReservas { get; set; }
        public bool PermiteProductos { get; set; }
        
        // Uso actual
        public int BarberosAfiliadosActuales { get; set; }
        public int ReservasEsteMes { get; set; }
        public int ProductosVentaActuales { get; set; }
        
        // Historial
        public string? HistorialPlanesJson { get; set; }
        
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
