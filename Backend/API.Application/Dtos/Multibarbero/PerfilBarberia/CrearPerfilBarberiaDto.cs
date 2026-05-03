namespace API.Application.Dtos.Multibarbero.PerfilBarberia
{
    public class CrearPerfilBarberiaDto
    {
        public Guid UsuarioId { get; set; }
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
        
        // Suscripción inicial
        public Guid PlanSuscripcionId { get; set; }
    }
}
