using API.Application.Dtos.Multibarbero.CategoriaProducto;
using API.Application.Services.Multibarbero.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers.Multibarbero;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[SwaggerTag("Gestión de Categorías de Productos")]
public class CategoriaProductoController : ControllerBase
{
    private readonly IProductoService _productoService;

    public CategoriaProductoController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    [SwaggerOperation("Obtener listado paginado de categorías de productos")]
    [ProducesResponseType(typeof(CategoriaProductoListadoPaginadoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerCategorias([FromQuery] CategoriaProductoFiltroDto filtro)
    {
        var resultado = await _productoService.ObtenerCategoriasPaginado(filtro);
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Obtener detalles de una categoría de producto")]
    [ProducesResponseType(typeof(CategoriaProductoDetallesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerCategoria(int id)
    {
        var categoria = await _productoService.ObtenerCategoriaDetalles(id);
        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    [HttpPost]
    [SwaggerOperation("Crear nueva categoría de producto")]
    [ProducesResponseType(typeof(CategoriaProductoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CrearCategoria([FromBody] CrearCategoriaProductoDto dto)
    {
        var categoriaCreada = await _productoService.CrearCategoria(dto);
        return CreatedAtAction(nameof(ObtenerCategoria), new { id = categoriaCreada.Id }, categoriaCreada);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Actualizar categoría de producto")]
    [ProducesResponseType(typeof(CategoriaProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] ActualizarCategoriaProductoDto dto)
    {
        if (id != dto.Id)
            return BadRequest("El ID de la ruta no coincide con el ID del cuerpo");

        var categoriaActualizada = await _productoService.ActualizarCategoria(dto);
        if (categoriaActualizada == null)
            return NotFound();

        return Ok(categoriaActualizada);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Eliminar lógica de categoría de producto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarCategoria(int id)
    {
        var eliminado = await _productoService.EliminarCategoria(id);
        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}
