using API.Application.Dtos.Multibarbero.Estadisticas;

namespace API.Application.Contracts.Multibarbero
{
    public interface IEstadisticaService
    {
        // Estadísticas de Barbero
        Task<EstadisticaBarberoDto> ObtenerEstadisticasBarberoAsync(Guid barberoId, int mes, int anio);
        Task<EstadisticaBarberoDto> ObtenerEstadisticasBarberoPeriodoAsync(Guid barberoId, DateTime fechaInicio, DateTime fechaFin);

        // Estadísticas de Barbería
        Task<EstadisticaBarberiaDto> ObtenerEstadisticasBarberiaAsync(Guid barberiaId, int mes, int anio);
        Task<EstadisticaBarberiaDto> ObtenerEstadisticasBarberiaPeriodoAsync(Guid barberiaId, DateTime fechaInicio, DateTime fechaFin);

        // Estadísticas de Productos
        Task<EstadisticaProductoDto> ObtenerEstadisticasProductoAsync(Guid productoId, int mes, int anio);

        // Dashboard para Comercial/Admin
        Task<object> ObtenerDashboardGeneralAsync();
        Task<object> ObtenerDashboardComercialAsync();

        // Reportes
        Task<List<EstadisticaBarberoDto>> ObtenerRankingBarberosAsync(int mes, int anio, int topN = 10);
        Task<List<EstadisticaBarberiaDto>> ObtenerRankingBarberiasAsync(int mes, int anio, int topN = 10);
    }
}
