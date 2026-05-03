using API.Data.Context;
using API.Data.Repositories.Base;
using API.Domain.Entities.Multibarbero;
using API.Domain.Interfaces.Repositories.Multibarbero;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero;

public class EstadisticaProductoRepository : RepositoryBase<EstadisticaProducto>, IEstadisticaProductoRepository
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
