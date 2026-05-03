namespace API.Application.Dtos.Multibarbero.PerfilBarberia
{
    public class PerfilBarberiaFiltroDto
    {
        public string? NombreComercial { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        
        // Filtros de ubicación
        public decimal? LatitudMin { get; set; }
        public decimal? LatitudMax { get; set; }
        public decimal? LongitudMin { get; set; }
        public decimal? LongitudMax { get; set; }
        
        // Filtros de plan
        public Guid? PlanSuscripcionId { get; set; }
        public bool? ConPlanActivo { get; set; }
        
        // Filtros de estado
        public bool? AceptaAfiliaciones { get; set; }
        public int? MinBarberosAfiliados { get; set; }
        public int? MaxBarberosAfiliados { get; set; }
        
        // Filtros de calificación
        public decimal? CalificacionMinima { get; set; }
        
        // Filtros de fecha
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        
        // Paginación y ordenamiento
        public int Pagina { get; set; } = 1;
        public int CantidadPorPagina { get; set; } = 10;
        public string? ColumnaOrden { get; set; }
        public bool OrdenDescendente { get; set; } = false;
    }
}
