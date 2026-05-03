using API.Application.Dtos.Multibarbero.PerfilBarberia;
using API.Application.Services.Multibarbero;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Application.Controllers.Multibarbero;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[SwaggerTag("Gestión de Perfiles de Barbería")]
public class PerfilBarberiaController : ControllerBase
{
    private readonly IPerfilBarberiaService _perfilBarberiaService;

    public PerfilBarberiaController(IPerfilBarberiaService perfilBarberiaService)
    {
        _perfilBarberiaService = perfilBarberiaService;
    }

    [HttpGet]
    [SwaggerOperation("Obtener todas las barberías (paginado)")]
    [ProducesResponseType(typeof(PerfilBarberiaListadoPaginadoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos([FromQuery] PerfilBarberiaFiltroDto filtro)
    {
        var resultado = await _perfilBarberiaService.ObtenerListadoPaginadoAsync(filtro);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Obtener detalles de una barbería por ID")]
    [ProducesResponseType(typeof(PerfilBarberiaDetallesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(Guid id)
    {
        var barberia = await _perfilBarberiaService.ObtenerDetalleAsync(id);
        if (barberia == null)
            return NotFound();

        return Ok(barberia);
    }

    [HttpGet("usuario/{usuarioId}")]
    [SwaggerOperation("Obtener perfil de barbería por usuario")]
    [ProducesResponseType(typeof(PerfilBarberiaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorUsuario(Guid usuarioId)
    {
        var barberia = await _perfilBarberiaService.ObtenerPorUsuarioIdAsync(usuarioId);
        if (barberia == null)
            return NotFound();

        return Ok(barberia);
    }

    [HttpGet("cercanas")]
    [SwaggerOperation("Obtener barberías cercanas por ubicación")]
    [ProducesResponseType(typeof(List<PerfilBarberiaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerCercanas(decimal latitud, decimal longitud, double radioKm = 5)
    {
        var barberias = await _perfilBarberiaService.ObtenerCercanasAsync(latitud, longitud, radioKm);
        return Ok(barberias);
    }

    [HttpPost]
    [SwaggerOperation("Crear nuevo perfil de barbería")]
    [ProducesResponseType(typeof(PerfilBarberiaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearPerfilBarberiaDto input)
    {
        try
        {
            var barberia = await _perfilBarberiaService.CrearAsync(input);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = barberia.Id }, barberia);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Actualizar perfil de barbería")]
    [ProducesResponseType(typeof(PerfilBarberiaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarPerfilBarberiaDto input)
    {
        try
        {
            var barberia = await _perfilBarberiaService.ActualizarAsync(input);
            return Ok(barberia);
        }
        catch (Exception ex) when (ex.Message.Contains("no encontrado"))
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Eliminar perfil de barbería")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        try
        {
            await _perfilBarberiaService.EliminarAsync(id);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("no encontrado"))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/barberos")]
    [SwaggerOperation("Obtener barberos afiliados a una barbería")]
    [ProducesResponseType(typeof(List<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerBarberosAfiliados(Guid id)
    {
        var barberos = await _perfilBarberiaService.ObtenerBarberosAfiliadosAsync(id);
        return Ok(barberos);
    }

    [HttpGet("{id}/servicios")]
    [SwaggerOperation("Obtener servicios de una barbería")]
    [ProducesResponseType(typeof(List<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerServicios(Guid id)
    {
        var servicios = await _perfilBarberiaService.ObtenerServiciosAsync(id);
        return Ok(servicios);
    }

    [HttpGet("{id}/estadisticas")]
    [SwaggerOperation("Obtener estadísticas de una barbería")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerEstadisticas(Guid id, [FromQuery] DateTime? fechaDesde, DateTime? fechaHasta)
    {
        var estadisticas = await _perfilBarberiaService.ObtenerEstadisticasAsync(id, fechaDesde, fechaHasta);
        return Ok(estadisticas);
    }
}
