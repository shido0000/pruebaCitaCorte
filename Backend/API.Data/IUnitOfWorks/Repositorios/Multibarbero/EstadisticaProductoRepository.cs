using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero;

public class EstadisticaProductoRepository : BaseRepository<EstadisticaProducto>, IEstadisticaProductoRepository
{
    public EstadisticaProductoRepository(ApiDbContext context) : base(context) { }

    public async Task<IEnumerable<EstadisticaProducto>> ObtenerPorProductoAsync(Guid productoId)
    {
        return await _dbSet.Where(e => e.ProductoId == productoId).OrderByDescending(e => e.Fecha).ToListAsync();
    }

    public async Task<EstadisticaProducto?> ObtenerUltimaAsync(Guid productoId)
    {
        return await _dbSet
            .Where(e => e.ProductoId == productoId)
            .OrderByDescending(e => e.Fecha)
            .FirstOrDefaultAsync();
    }
}
