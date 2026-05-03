namespace API.Application.Dtos.Multibarbero.Estadisticas;

public class EstadisticaFiltroDto
{
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? IdBarbero { get; set; }
    public int? IdBarberia { get; set; }
    public int? IdProducto { get; set; }
    public string? TipoEstadistica { get; set; } // "barbero", "barberia", "producto"
}
