using API.Application.Contracts.Multibarbero;
using API.Application.Dtos.Multibarbero.Producto;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services.Multibarbero
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ICategoriaProductoRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public ProductoService(
            IProductoRepository productoRepository,
            ICategoriaProductoRepository categoriaRepository,
            IMapper mapper)
        {
            _productoRepository = productoRepository;
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<ProductoDto> ObtenerPorIdAsync(Guid id)
        {
            var producto = await _productoRepository.ObtenerPorIdAsync(id);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");

            return _mapper.Map<ProductoDto>(producto);
        }

        public async Task<ProductoDetallesDto> ObtenerDetallesAsync(Guid id)
        {
            var producto = await _productoRepository
                .Queryable
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");

            return _mapper.Map<ProductoDetallesDto>(producto);
        }

        public async Task<ProductoListadoPaginadoDto> ListarPaginadoAsync(ProductoFiltroDto filtro)
        {
            var query = _productoRepository.Queryable;

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(filtro.Nombre))
                query = query.Where(p => p.Nombre.Contains(filtro.Nombre));

            if (filtro.BarberoId.HasValue)
                query = query.Where(p => p.BarberoId == filtro.BarberoId);

            if (filtro.CategoriaId.HasValue)
                query = query.Where(p => p.CategoriaId == filtro.CategoriaId);

            if (filtro.PrecioMin.HasValue)
                query = query.Where(p => p.Precio >= filtro.PrecioMin);

            if (filtro.PrecioMax.HasValue)
                query = query.Where(p => p.Precio <= filtro.PrecioMax);

            if (filtro.SoloActivos.HasValue && filtro.SoloActivos.Value)
                query = query.Where(p => p.Activo);

            // Ordenamiento
            query = !string.IsNullOrWhiteSpace(filtro.ColumnaOrden) && filtro.ColumnaOrden.ToLower() == "precio"
                ? (filtro.OrdenDescendente ? query.OrderByDescending(p => p.Precio) : query.OrderBy(p => p.Precio))
                : (filtro.OrdenDescendente ? query.OrderByDescending(p => p.FechaCreacion) : query.OrderBy(p => p.FechaCreacion));

            var totalRegistros = await query.CountAsync();
            var productos = await query
                .Skip((filtro.Pagina - 1) * filtro.CantidadPorPagina)
                .Take(filtro.CantidadPorPagina)
                .ToListAsync();

            return new ProductoListadoPaginadoDto
            {
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)filtro.CantidadPorPagina),
                PaginaActual = filtro.Pagina,
                CantidadPorPagina = filtro.CantidadPorPagina,
                Productos = _mapper.Map<List<ProductoDto>>(productos)
            };
        }

        public async Task<ProductoDto> CrearAsync(CrearProductoDto dto)
        {
            // Validar categoría
            var categoria = await _categoriaRepository.ObtenerPorIdAsync(dto.CategoriaId);
            if (categoria == null)
                throw new ArgumentException($"Categoría con ID {dto.CategoriaId} no encontrada");

            var producto = _mapper.Map<Producto>(dto);
            producto.Activo = true;
            producto.FechaCreacion = DateTime.UtcNow;

            await _productoRepository.CrearAsync(producto);
            await _productoRepository.GuardarCambiosAsync();

            return _mapper.Map<ProductoDto>(producto);
        }

        public async Task<ProductoDto> ActualizarAsync(Guid id, ActualizarProductoDto dto)
        {
            var producto = await _productoRepository.ObtenerPorIdAsync(id);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");

            // Validar categoría si se proporciona
            if (dto.CategoriaId.HasValue)
            {
                var categoria = await _categoriaRepository.ObtenerPorIdAsync(dto.CategoriaId.Value);
                if (categoria == null)
                    throw new ArgumentException($"Categoría con ID {dto.CategoriaId} no encontrada");
            }

            _mapper.Map(dto, producto);
            producto.FechaActualizacion = DateTime.UtcNow;

            await _productoRepository.ActualizarAsync(producto);
            await _productoRepository.GuardarCambiosAsync();

            return _mapper.Map<ProductoDto>(producto);
        }

        public async Task EliminarAsync(Guid id)
        {
            var producto = await _productoRepository.ObtenerPorIdAsync(id);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");

            await _productoRepository.EliminarAsync(producto);
            await _productoRepository.GuardarCambiosAsync();
        }

        public async Task<ProductoDto> ActivarDesactivarAsync(Guid id, bool activo)
        {
            var producto = await _productoRepository.ObtenerPorIdAsync(id);
            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado");

            producto.Activo = activo;
            producto.FechaActualizacion = DateTime.UtcNow;

            await _productoRepository.ActualizarAsync(producto);
            await _productoRepository.GuardarCambiosAsync();

            return _mapper.Map<ProductoDto>(producto);
        }

        public async Task<int> ObtenerTotalProductosBarberoAsync(Guid barberoId)
        {
            return await _productoRepository.Queryable.CountAsync(p => p.BarberoId == barberoId && p.Activo);
        }
    }
}
