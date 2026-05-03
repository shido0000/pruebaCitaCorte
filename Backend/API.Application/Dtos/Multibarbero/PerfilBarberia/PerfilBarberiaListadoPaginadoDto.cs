namespace API.Application.Dtos.Multibarbero.PerfilBarberia
{
    public class PerfilBarberiaListadoPaginadoDto
    {
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public int CantidadPorPagina { get; set; }
        public List<PerfilBarberiaResumenDto> Barberias { get; set; } = new();
    }

    public class PerfilBarberiaResumenDto
    {
        public Guid Id { get; set; }
        public string NombreComercial { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Logo { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        public decimal CalificacionPromedio { get; set; }
        public int TotalReseñas { get; set; }

        public string? PlanSuscripcionNombre { get; set; }
        public DateTime? FechaVencimientoPlan { get; set; }

        public int TotalBarberosAfiliados { get; set; }
        public int MaxBarberosPermitidos { get; set; }

        public bool TieneCupoDisponible => MaxBarberosPermitidos > TotalBarberosAfiliados;

        public DateTime FechaCreacion { get; set; }
    }
}
