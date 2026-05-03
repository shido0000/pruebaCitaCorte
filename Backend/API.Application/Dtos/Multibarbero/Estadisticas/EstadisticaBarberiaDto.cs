namespace API.Application.Dtos.Multibarbero.Estadisticas;

public class EstadisticaBarberiaDto
{
    public int IdBarberia { get; set; }
    public string NombreBarberia { get; set; } = string.Empty;
    public int TotalBarberos { get; set; }
    public int BarberosActivos { get; set; }
    public int TotalReservas { get; set; }
    public decimal IngresosTotales { get; set; }
    public double CalificacionPromedio { get; set; }
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFin { get; set; }
}
