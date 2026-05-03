namespace API.Application.Dtos.Multibarbero.Afiliaciones
{
    public class AfiliacionDetallesDto
    {
        public Guid Id { get; set; }
        
        // Datos del barbero
        public Guid BarberoId { get; set; }
        public string? BarberoNombre { get; set; }
        public string? BarberoFotoUrl { get; set; }
        public decimal? BarberoCalificacionPromedio { get; set; }
        public int? BarberoExperienciaAnios { get; set; }
        public string? BarberoEspecialidades { get; set; }
        
        // Datos de la barbería
        public Guid BarberiaId { get; set; }
        public string? BarberiaNombre { get; set; }
        public string? BarberiaDireccion { get; set; }
        public string? BarberiaTelefono { get; set; }
        public string? BarberiaLogo { get; set; }
        
        // Datos de la afiliación
        public DateTime FechaAfiliacion { get; set; }
        public bool Activo { get; set; }
        public bool EsPrincipal { get; set; }
        public DateTime? FechaFinAfiliacion { get; set; }
        public string? MotivoFin { get; set; }
        
        // Estadísticas
        public int TotalReservasCompletadas { get; set; }
        public decimal IngresosGenerados { get; set; }
    }
}
