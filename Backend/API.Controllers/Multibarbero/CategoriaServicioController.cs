using API.Application.Dtos.Multibarbero.CategoriaServicio;
using API.Application.Services.Multibarbero.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers.Multibarbero;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[SwaggerTag("Gestión de Categorías de Servicios")]
public class CategoriaServicioController : ControllerBase
{
    private readonly IServicioService _servicioService;

    public CategoriaServicioController(IServicioService servicioService)
    {
        _servicioService = servicioService;
    }

    [HttpGet]
    [SwaggerOperation("Obtener listado paginado de categorías de servicio")]
    [ProducesResponseType(typeof(CategoriaServicioListadoPaginadoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerCategorias([FromQuery] CategoriaServicioFiltroDto filtro)
    {
        var resultado = await _servicioService.ObtenerCategoriasPaginado(filtro);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Obtener detalles de una categoría de servicio")]
    [ProducesResponseType(typeof(CategoriaServicioDetallesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerCategoria(int id)
    {
        var categoria = await _servicioService.ObtenerCategoriaDetalles(id);
        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    [HttpPost]
    [SwaggerOperation("Crear nueva categoría de servicio")]
    [ProducesResponseType(typeof(CategoriaServicioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearCategoria([FromBody] CrearCategoriaServicioDto dto)
    {
        var categoriaCreada = await _servicioService.CrearCategoria(dto);
        return CreatedAtAction(nameof(ObtenerCategoria), new { id = categoriaCreada.Id }, categoriaCreada);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Actualizar categoría de servicio")]
    [ProducesResponseType(typeof(CategoriaServicioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] ActualizarCategoriaServicioDto dto)
    {
        if (id != dto.Id)
            return BadRequest("El ID de la ruta no coincide con el ID del cuerpo");

        var categoriaActualizada = await _servicioService.ActualizarCategoria(dto);
        if (categoriaActualizada == null)
            return NotFound();

        return Ok(categoriaActualizada);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Eliminar lógica de categoría de servicio")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarCategoria(int id)
    {
        var eliminado = await _servicioService.EliminarCategoria(id);
        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}
