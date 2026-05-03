namespace API.Application.Dtos.Multibarbero.PerfilBarberia
{
    public class PerfilBarberiaDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? UsuarioEmail { get; set; }
        
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
        public string DiasLaborablesJson { get; set; } = "[]";
        
        // Capacidad y límites
        public int? CapacidadMaxima { get; set; }
        public int MaxBarberosPermitidos { get; set; }
        
        public decimal CalificacionPromedio { get; set; }
        public int TotalReseñas { get; set; }
        
        // Suscripción
        public Guid? PlanSuscripcionId { get; set; }
        public string? PlanSuscripcionNombre { get; set; }
        public DateTime? FechaInicioPlan { get; set; }
        public DateTime? FechaVencimientoPlan { get; set; }
        public string? EstadoSolicitudCambioPlan { get; set; }
        
        public int TotalBarberosAfiliados { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
