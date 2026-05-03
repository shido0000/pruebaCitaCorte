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

public class ServicioRepository : RepositoryBase<Servicio>, IServicioRepository
{
    public ServicioRepository(ApiDbContext context) : base(context) { }

    public async Task<IEnumerable<Servicio>> ObtenerPorBarberiaAsync(Guid barberiaId)
    {
        return await _dbSet.Where(s => s.IdBarberia == barberiaId && s.Estado).ToListAsync();
    }

    public async Task<IEnumerable<Servicio>> ObtenerPorBarberiasAsync(List<Guid> barberiasIds)
    {
        return await _dbSet.Where(s => barberiasIds.Contains(s.IdBarberia) && s.Estado).ToListAsync();
    }

    public async Task<IEnumerable<Servicio>> ObtenerPorCategoriaAsync(Guid categoriaId)
    {
        return await _dbSet.Where(s => s.IdCategoriaServicio == categoriaId && s.Estado).ToListAsync();
    }

    public async Task<IEnumerable<Servicio>> ObtenerPorCategoriaYBarberiasAsync(Guid categoriaId, List<Guid> barberiasIds)
    {
        return await _dbSet
            .Where(s => s.IdCategoriaServicio == categoriaId && barberiasIds.Contains(s.IdBarberia) && s.Estado)
            .ToListAsync();
    }

    public async Task<bool> ExisteNombreEnBarberiaAsync(string nombre, Guid barberiaId, Guid? idExcluir = null)
    {
        var query = _dbSet.Where(s => s.Nombre == nombre && s.IdBarberia == barberiaId && s.Estado);
        
        if (idExcluir.HasValue)
            query = query.Where(s => s.Id != idExcluir.Value);
        
        return await query.AnyAsync();
    }
}
