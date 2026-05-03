using API.Application.Dtos.Multibarbero;
using API.Application.Services.Multibarbero;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Application.Controllers.Multibarbero;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[SwaggerTag("Gestión de Servicios de Barbería")]
public class ServicioController : ControllerBase
{
    private readonly IServicioService _servicioService;

    public ServicioController(IServicioService servicioService)
    {
        _servicioService = servicioService;
    }

    [HttpGet]
    [SwaggerOperation("Obtener todos los servicios")]
    [ProducesResponseType(typeof(List<ServicioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos()
    {
        var servicios = await _servicioService.ObtenerTodosAsync();
        return Ok(servicios);
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Obtener servicio por ID")]
    [ProducesResponseType(typeof(ServicioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(Guid id)
    {
        var servicio = await _servicioService.ObtenerPorIdAsync(id);
        if (servicio == null)
            return NotFound();

        return Ok(servicio);
    }

    [HttpGet("barberia/{idBarberia}")]
    [SwaggerOperation("Obtener servicios por barbería")]
    [ProducesResponseType(typeof(List<ServicioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorBarberia(Guid idBarberia)
    {
        var servicios = await _servicioService.ObtenerPorBarberiaAsync(idBarberia);
        return Ok(servicios);
    }

    [HttpGet("categoria/{idCategoria}")]
    [SwaggerOperation("Obtener servicios por categoría")]
    [ProducesResponseType(typeof(List<ServicioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorCategoria(Guid idCategoria)
    {
        var servicios = await _servicioService.ObtenerPorCategoriaAsync(idCategoria);
        return Ok(servicios);
    }

    [HttpPost]
    [SwaggerOperation("Crear nuevo servicio")]
    [ProducesResponseType(typeof(ServicioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearServicioInputDto input)
    {
        try
        {
            var servicio = await _servicioService.CrearAsync(input);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = servicio.Id }, servicio);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Actualizar servicio existente")]
    [ProducesResponseType(typeof(ServicioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarServicioInputDto input)
    {
        try
        {
            var servicio = await _servicioService.ActualizarAsync(input);
            return Ok(servicio);
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
    [SwaggerOperation("Eliminar servicio")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        try
        {
            await _servicioService.EliminarAsync(id);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("no encontrado"))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("verificar-nombre")]
    [SwaggerOperation("Verificar si existe un nombre de servicio en una barbería")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerificarNombre(string nombre, Guid idBarberia, Guid? idExcluir = null)
    {
        var existe = await _servicioService.ExisteNombreEnBarberiaAsync(nombre, idBarberia, idExcluir);
        return Ok(existe);
    }
}
