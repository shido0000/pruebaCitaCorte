namespace API.Application.Dtos.Multibarbero.Estadisticas;

public class EstadisticaBarberoDto
{
    public int IdBarbero { get; set; }
    public string NombreBarbero { get; set; } = string.Empty;
    public int TotalReservas { get; set; }
    public int ReservasCompletadas { get; set; }
    public int ReservasCanceladas { get; set; }
    public decimal IngresosTotales { get; set; }
    public double CalificacionPromedio { get; set; }
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFin { get; set; }
}
