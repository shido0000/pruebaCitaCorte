using API.Application.Contracts.Multibarbero;
using API.Application.Dtos.Multibarbero.Producto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Application.Controllers.Multibarbero;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[SwaggerTag("Gestión de Productos de Barbería")]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductoController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    [SwaggerOperation("Obtener todos los productos")]
    [ProducesResponseType(typeof(List<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos([FromQuery] ProductoFiltroDto filtro)
    {
        var productos = await _productoService.ObtenerTodosAsync(filtro);
        return Ok(productos);
    }

    [HttpGet("{id}")]
    [SwaggerOperation("Obtener producto por ID")]
    [ProducesResponseType(typeof(ProductoDetallesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(Guid id)
    {
        var producto = await _productoService.ObtenerDetalleAsync(id);
        if (producto == null)
            return NotFound();

        return Ok(producto);
    }

    [HttpGet("barberia/{idBarberia}")]
    [SwaggerOperation("Obtener productos por barbería")]
    [ProducesResponseType(typeof(List<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorBarberia(Guid idBarberia, [FromQuery] ProductoFiltroDto filtro)
    {
        var productos = await _productoService.ObtenerPorBarberiaAsync(idBarberia, filtro);
        return Ok(productos);
    }

    [HttpGet("categoria/{idCategoria}")]
    [SwaggerOperation("Obtener productos por categoría")]
    [ProducesResponseType(typeof(List<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerPorCategoria(Guid idCategoria, [FromQuery] ProductoFiltroDto filtro)
    {
        var productos = await _productoService.ObtenerPorCategoriaAsync(idCategoria, filtro);
        return Ok(productos);
    }

    [HttpPost]
    [SwaggerOperation("Crear nuevo producto")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearProductoDto input)
    {
        try
        {
            var producto = await _productoService.CrearAsync(input);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Actualizar producto existente")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarProductoDto input)
    {
        try
        {
            var producto = await _productoService.ActualizarAsync(input);
            return Ok(producto);
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
    [SwaggerOperation("Eliminar producto")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        try
        {
            await _productoService.EliminarAsync(id);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("no encontrado"))
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("verificar-nombre")]
    [SwaggerOperation("Verificar si existe un nombre de producto en una barbería")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerificarNombre(string nombre, Guid idBarberia, Guid? idExcluir = null)
    {
        var existe = await _productoService.ExisteNombreEnBarberiaAsync(nombre, idBarberia, idExcluir);
        return Ok(existe);
    }

    [HttpPatch("{id}/stock")]
    [SwaggerOperation("Actualizar stock de producto")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ActualizarStock(Guid id, [FromBody] int cantidad)
    {
        try
        {
            var producto = await _productoService.ActualizarStockAsync(id, cantidad);
            return Ok(producto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
