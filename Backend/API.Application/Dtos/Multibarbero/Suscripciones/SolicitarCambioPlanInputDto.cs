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
        /// Motivo o comentario opcional de la solicitud
        /// </summary>
        public string? MotivoSolicitud { get; set; }
    }
}
