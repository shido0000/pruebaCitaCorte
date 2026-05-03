using API.Application.Contracts.Multibarbero;
using API.Application.Dtos.Multibarbero.Estadisticas;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Application.Services.Multibarbero
{
    public class EstadisticaService : IEstadisticaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IServicioRepository _servicioRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IPerfilBarberoRepository _barberoRepository;
        private readonly IPerfilBarberiaRepository _barberiaRepository;

        public EstadisticaService(
            IReservaRepository reservaRepository,
            IServicioRepository servicioRepository,
            IProductoRepository productoRepository,
            IPerfilBarberoRepository barberoRepository,
            IPerfilBarberiaRepository barberiaRepository)
        {
            _reservaRepository = reservaRepository;
            _servicioRepository = servicioRepository;
            _productoRepository = productoRepository;
            _barberoRepository = barberoRepository;
            _barberiaRepository = barberiaRepository;
        }

        public async Task<ResumenEstadisticasDto> ObtenerResumen(EstadisticaFiltroDto filtro)
        {
            var reservas = await _reservaRepository.ObtenerPorFiltrosAsync(filtro);
            var totalReservas = reservas.Count();
            var reservasCompletadas = reservas.Count(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada);
            var reservasCanceladas = reservas.Count(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Cancelada);

            var ingresosTotales = reservas
                .Where(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada)
                .Sum(r => r.PrecioTotal ?? 0);

            return new ResumenEstadisticasDto
            {
                TotalReservas = totalReservas,
                ReservasCompletadas = reservasCompletadas,
                ReservasCanceladas = reservasCanceladas,
                TasaCompletacion = totalReservas > 0 ? (double)reservasCompletadas / totalReservas * 100 : 0,
                IngresosTotales = ingresosTotales,
                TicketPromedio = reservasCompletadas > 0 ? ingresosTotales / reservasCompletadas : 0,
                PeriodoInicio = filtro.FechaDesde,
                PeriodoFin = filtro.FechaHasta
            };
        }

        public async Task<List<EstadisticaBarberoDto>> ObtenerEstadisticasBarberos(EstadisticaFiltroDto filtro)
        {
            var reservas = await _reservaRepository.ObtenerPorFiltrosAsync(filtro);

            return reservas
                .GroupBy(r => r.IdBarbero)
                .Select(g => new EstadisticaBarberoDto
                {
                    BarberoId = g.Key,
                    TotalReservas = g.Count(),
                    ReservasCompletadas = g.Count(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada),
                    IngresosGenerados = g.Where(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada).Sum(r => r.PrecioTotal ?? 0),
                    CalificacionPromedio = g.Average(r => r.Calificacion ?? 0),
                    TotalCalificaciones = g.Count(r => r.Calificacion.HasValue)
                })
                .ToList();
        }

        public async Task<List<EstadisticaBarberiaDto>> ObtenerEstadisticasBarberias(EstadisticaFiltroDto filtro)
        {
            var reservas = await _reservaRepository.ObtenerPorFiltrosAsync(filtro);

            return reservas
                .GroupBy(r => r.IdBarberia)
                .Select(g => new EstadisticaBarberiaDto
                {
                    BarberiaId = g.Key,
                    TotalReservas = g.Count(),
                    ReservasCompletadas = g.Count(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada),
                    IngresosGenerados = g.Where(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada).Sum(r => r.PrecioTotal ?? 0),
                    CalificacionPromedio = g.Average(r => r.Calificacion ?? 0),
                    TotalCalificaciones = g.Count(r => r.Calificacion.HasValue),
                    ServiciosMasSolicitados = g.GroupBy(r => r.IdServicio)
                        .OrderByDescending(grp => grp.Count())
                        .Take(5)
                        .Select(grp => grp.Key)
                        .ToList()
                })
                .ToList();
        }

        public async Task<List<EstadisticaProductoDto>> ObtenerEstadisticasProductos(EstadisticaFiltroDto filtro)
        {
            // Implementación básica - se puede expandir con datos reales de ventas
            var productos = await _productoRepository.ObtenerTodosAsync();

            return productos.Select(p => new EstadisticaProductoDto
            {
                ProductoId = p.Id,
                NombreProducto = p.Nombre,
                TotalVentas = 0, // Se debe implementar con datos reales
                IngresosGenerados = 0, // Se debe implementar con datos reales
                StockActual = p.Stock,
                CategoriaId = p.IdCategoria
            }).ToList();
        }

        public async Task<object> ObtenerDashboardBarbero(int idBarbero, EstadisticaFiltroDto filtro)
        {
            var reservas = await _reservaRepository.ObtenerPorFiltrosAsync(filtro);
            var reservasBarbero = reservas.Where(r => r.IdBarbero == idBarbero).ToList();

            var totalReservas = reservasBarbero.Count;
            var completadas = reservasBarbero.Count(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada);
            var ingresos = reservasBarbero.Where(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada).Sum(r => r.PrecioTotal ?? 0);

            return new
            {
                BarberoId = idBarbero,
                Resumen = new
                {
                    TotalReservas = totalReservas,
                    ReservasCompletadas = completadas,
                    IngresosTotales = ingresos,
                    CalificacionPromedio = reservasBarbero.Average(r => r.Calificacion ?? 0)
                },
                EvolucionMensual = reservasBarbero
                    .GroupBy(r => r.Fecha.Date.Month)
                    .Select(g => new
                    {
                        Mes = g.Key,
                        Cantidad = g.Count(),
                        Ingresos = g.Where(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada).Sum(r => r.PrecioTotal ?? 0)
                    }),
                ServiciosRealizados = reservasBarbero
                    .GroupBy(r => r.IdServicio)
                    .Select(g => new
                    {
                        ServicioId = g.Key,
                        Cantidad = g.Count()
                    })
                    .OrderByDescending(x => x.Cantidad)
                    .Take(10)
            };
        }

        public async Task<object> ObtenerDashboardBarberia(int idBarberia, EstadisticaFiltroDto filtro)
        {
            var reservas = await _reservaRepository.ObtenerPorFiltrosAsync(filtro);
            var reservasBarberia = reservas.Where(r => r.IdBarberia == idBarberia).ToList();

            var totalReservas = reservasBarberia.Count;
            var completadas = reservasBarberia.Count(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada);
            var ingresos = reservasBarberia.Where(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada).Sum(r => r.PrecioTotal ?? 0);

            // Obtener barberos afiliados
            var barberos = await _barberoRepository.ObtenerPorBarberiaAsync(idBarberia);

            return new
            {
                BarberiaId = idBarberia,
                Resumen = new
                {
                    TotalReservas = totalReservas,
                    ReservasCompletadas = completadas,
                    IngresosTotales = ingresos,
                    CalificacionPromedio = reservasBarberia.Average(r => r.Calificacion ?? 0),
                    TotalBarberos = barberos.Count()
                },
                EvolucionMensual = reservasBarberia
                    .GroupBy(r => r.Fecha.Date.Month)
                    .Select(g => new
                    {
                        Mes = g.Key,
                        Cantidad = g.Count(),
                        Ingresos = g.Where(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada).Sum(r => r.PrecioTotal ?? 0)
                    }),
                TopBarberos = reservasBarberia
                    .GroupBy(r => r.IdBarbero)
                    .Select(g => new
                    {
                        BarberoId = g.Key,
                        CantidadReservas = g.Count(),
                        IngresosGenerados = g.Where(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada).Sum(r => r.PrecioTotal ?? 0)
                    })
                    .OrderByDescending(x => x.CantidadReservas)
                    .Take(5),
                ServiciosMasPopulares = reservasBarberia
                    .GroupBy(r => r.IdServicio)
                    .Select(g => new
                    {
                        ServicioId = g.Key,
                        Cantidad = g.Count(),
                        Ingresos = g.Where(r => r.Estado == Data.Enum.Multibarbero.EstadoReserva.Completada).Sum(r => r.PrecioTotal ?? 0)
                    })
                    .OrderByDescending(x => x.Cantidad)
                    .Take(10)
            };
        }
    }
}
