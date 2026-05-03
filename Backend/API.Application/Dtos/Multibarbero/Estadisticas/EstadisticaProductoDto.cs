namespace API.Application.Dtos.Multibarbero.Estadisticas;

public class EstadisticaProductoDto
{
    public int IdProducto { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public int TotalVentas { get; set; }
    public decimal IngresosTotales { get; set; }
    public int StockActual { get; set; }
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFin { get; set; }
}
