namespace API.Application.Dtos.Multibarbero.Suscripciones
{
    /// <summary>
    /// DTO para responder a una solicitud de cambio de plan
    /// </summary>
    public class ResponderCambioPlanInputDto
    {
        /// <summary>
        /// ID de la solicitud de cambio de plan
        /// </summary>
        public Guid SolicitudId { get; set; }

        /// <summary>
        /// Decisión: true = Aprobar, false = Rechazar
        /// </summary>
        public bool Aprobado { get; set; }

        /// <summary>
        /// Motivo del rechazo (obligatorio si se rechaza)
        /// </summary>
        public string? MotivoRechazo { get; set; }

        /// <summary>
        /// Notas adicionales opcionales
        /// </summary>
        public string? NotasAdicionales { get; set; }

        /// <summary>
        /// ID del usuario que responde (Admin o Comercial)
        /// </summary>
        public Guid UsuarioRespuestaId { get; set; }
    }
}
