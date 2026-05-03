namespace API.Application.Dtos.Multibarbero.Afiliaciones
{
    /// <summary>
    /// DTO para detalles de una solicitud de afiliación
    /// </summary>
    public class SolicitudAfiliacionDto
    {
        public Guid Id { get; set; }
        
        // Datos del barbero
        public Guid BarberoId { get; set; }
        public string? BarberoNombre { get; set; }
        public string? BarberoFotoUrl { get; set; }
        public string? BarberoEmail { get; set; }
        
        // Datos de la barbería
        public Guid BarberiaId { get; set; }
        public string? BarberiaNombre { get; set; }
        public string? BarberiaDireccion { get; set; }
        
        // Estado de la solicitud
        public string Estado { get; set; } = string.Empty; // Pendiente, Aprobada, Rechazada, Retirada
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string? MotivoRechazo { get; set; }
        public string? Notas { get; set; }
        
        // Usuario que respondió
        public Guid? RespondidoPorId { get; set; }
        public string? RespondidoPorNombre { get; set; }
        
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
