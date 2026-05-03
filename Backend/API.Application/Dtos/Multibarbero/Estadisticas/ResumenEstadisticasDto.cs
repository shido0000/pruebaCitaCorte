namespace API.Application.Dtos.Multibarbero.Estadisticas;

public class ResumenEstadisticasDto
{
    public int TotalBarberos { get; set; }
    public int TotalBarberias { get; set; }
    public int TotalReservas { get; set; }
    public decimal IngresosTotales { get; set; }
    public double CalificacionPromedioGlobal { get; set; }
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFin { get; set; }
}
