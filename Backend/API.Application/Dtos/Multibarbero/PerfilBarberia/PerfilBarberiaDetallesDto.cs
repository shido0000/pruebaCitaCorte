namespace API.Application.Dtos.Multibarbero.PerfilBarberia
{
    public class PerfilBarberiaDetallesDto : PerfilBarberiaDto
    {
        // Información adicional para detalles
        public string? NombreUsuarioPropietario { get; set; }
        public string? EmailUsuarioPropietario { get; set; }
        
        // Lista de barberos afiliados (resumida)
        public List<BarberoAfiliadoResumenDto> BarberosAfiliados { get; set; } = new();
        
        // Servicios ofrecidos
        public List<ServicioResumenDto> Servicios { get; set; } = new();
        
        // Historial de planes
        public string? HistorialPlanesJson { get; set; }
        
        // Configuración avanzada
        public bool AceptaSolicitudesAfiliacion { get; set; }
        public bool NotificacionesActivas { get; set; }
    }

    public class BarberoAfiliadoResumenDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? FotoPerfil { get; set; }
        public decimal CalificacionPromedio { get; set; }
        public DateTime FechaAfiliacion { get; set; }
        public bool EsPrincipal { get; set; }
    }

    public class ServicioResumenDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }
        public bool Activo { get; set; }
    }
}
