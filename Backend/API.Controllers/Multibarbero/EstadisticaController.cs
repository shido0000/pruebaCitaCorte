using API.Application.Dtos.Multibarbero.Estadisticas;
using API.Application.Services.Multibarbero.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers.Multibarbero;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[SwaggerTag("Gestión de Estadísticas y Reportes")]
public class EstadisticaController : ControllerBase
{
    private readonly IEstadisticaService _estadisticaService;

    public EstadisticaController(IEstadisticaService estadisticaService)
    {
        _estadisticaService = estadisticaService;
    }

    [HttpGet("resumen")]
    [SwaggerOperation("Obtener resumen general de estadísticas")]
    [ProducesResponseType(typeof(ResumenEstadisticasDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerResumen([FromQuery] EstadisticaFiltroDto filtro)
    {
        var resumen = await _estadisticaService.ObtenerResumen(filtro);
        return Ok(resumen);
    }

    [HttpGet("barbero")]
    [SwaggerOperation("Obtener estadísticas por barbero")]
    [ProducesResponseType(typeof(List<EstadisticaBarberoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerEstadisticasBarberos([FromQuery] EstadisticaFiltroDto filtro)
    {
        var estadisticas = await _estadisticaService.ObtenerEstadisticasBarberos(filtro);
        return Ok(estadisticas);
    }

    [HttpGet("barberia")]
    [SwaggerOperation("Obtener estadísticas por barbería")]
    [ProducesResponseType(typeof(List<EstadisticaBarberiaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerEstadisticasBarberias([FromQuery] EstadisticaFiltroDto filtro)
    {
        var estadisticas = await _estadisticaService.ObtenerEstadisticasBarberias(filtro);
        return Ok(estadisticas);
    }

    [HttpGet("producto")]
    [SwaggerOperation("Obtener estadísticas por producto")]
    [ProducesResponseType(typeof(List<EstadisticaProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerEstadisticasProductos([FromQuery] EstadisticaFiltroDto filtro)
    {
        var estadisticas = await _estadisticaService.ObtenerEstadisticasProductos(filtro);
        return Ok(estadisticas);
    }

    [HttpGet("dashboard/barbero/{idBarbero}")]
    [SwaggerOperation("Obtener datos para dashboard de barbero")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerDashboardBarbero(int idBarbero, [FromQuery] EstadisticaFiltroDto filtro)
    {
        var dashboard = await _estadisticaService.ObtenerDashboardBarbero(idBarbero, filtro);
        return Ok(dashboard);
    }

    [HttpGet("dashboard/barberia/{idBarberia}")]
    [SwaggerOperation("Obtener datos para dashboard de barbería")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerDashboardBarberia(int idBarberia, [FromQuery] EstadisticaFiltroDto filtro)
    {
        var dashboard = await _estadisticaService.ObtenerDashboardBarberia(idBarberia, filtro);
        return Ok(dashboard);
    }
}
