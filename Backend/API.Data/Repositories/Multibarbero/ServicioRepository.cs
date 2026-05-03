using API.Data.Context;
using API.Data.Repositories.Base;
using API.Domain.Entities.Multibarbero;
using API.Domain.Interfaces.Repositories.Multibarbero;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Repositories.Multibarbero;

public class ServicioRepository : RepositoryBase<Servicio>, IServicioRepository
{
    public ServicioRepository(ApiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Servicio>> ObtenerPorBarberiaAsync(Guid idBarberia)
    {
        return await _dbSet
            .Where(s => s.IdBarberia == idBarberia && s.Estado)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Servicio>> ObtenerPorBarberiasAsync(IEnumerable<Guid> idBarberias)
    {
        return await _dbSet
            .Where(s => idBarberias.Contains(s.IdBarberia) && s.Estado)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Servicio>> ObtenerPorCategoriaAsync(Guid idCategoria)
    {
        return await _dbSet
            .Where(s => s.IdCategoriaServicio == idCategoria && s.Estado)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Servicio>> ObtenerPorCategoriaYBarberiasAsync(Guid idCategoria, IEnumerable<Guid> idBarberias)
    {
        return await _dbSet
            .Where(s => s.IdCategoriaServicio == idCategoria && idBarberias.Contains(s.IdBarberia) && s.Estado)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
    }

    public async Task<bool> ExisteNombreEnBarberiaAsync(string nombre, Guid idBarberia, Guid? idExcluir = null)
    {
        var query = _dbSet.Where(s => s.Nombre.ToLower() == nombre.ToLower() && s.IdBarberia == idBarberia);
        
        if (idExcluir.HasValue)
        {
            query = query.Where(s => s.Id != idExcluir.Value);
        }

        return await query.AnyAsync();
    }
}
