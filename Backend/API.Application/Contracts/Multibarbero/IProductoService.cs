using API.Application.Dtos.Multibarbero.Producto;

namespace API.Application.Contracts.Multibarbero
{
    public interface IProductoService
    {
        Task<ProductoDto> ObtenerPorIdAsync(Guid id);
        Task<ProductoDetallesDto> ObtenerDetallesAsync(Guid id);
        Task<ProductoListadoPaginadoDto> ListarPaginadoAsync(ProductoFiltroDto filtro);
        Task<ProductoDto> CrearAsync(CrearProductoDto dto);
        Task<ProductoDto> ActualizarAsync(Guid id, ActualizarProductoDto dto);
        Task EliminarAsync(Guid id);
        Task<ProductoDto> ActivarDesactivarAsync(Guid id, bool activo);
        Task<int> ObtenerTotalProductosBarberoAsync(Guid barberoId);
    }
}
