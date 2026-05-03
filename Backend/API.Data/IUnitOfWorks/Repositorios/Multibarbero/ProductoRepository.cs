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

public class ProductoRepository : RepositoryBase<Producto>, IProductoRepository
{
    public ProductoRepository(ApiDbContext context) : base(context) { }

    public async Task<IEnumerable<Producto>> ObtenerPorBarberiaAsync(Guid barberiaId)
    {
        return await _dbSet.Where(p => p.IdBarberia == barberiaId && p.Estado).ToListAsync();
    }

    public async Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(Guid categoriaId)
    {
        return await _dbSet.Where(p => p.IdCategoriaProducto == categoriaId && p.Estado).ToListAsync();
    }

    public async Task<bool> ExisteNombreEnBarberiaAsync(string nombre, Guid barberiaId, Guid? idExcluir = null)
    {
        var query = _dbSet.Where(p => p.Nombre == nombre && p.IdBarberia == barberiaId && p.Estado);
        
        if (idExcluir.HasValue)
            query = query.Where(p => p.Id != idExcluir.Value);
        
        return await query.AnyAsync();
    }
}
