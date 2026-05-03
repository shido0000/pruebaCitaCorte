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

public class EstadisticaBarberoRepository : RepositoryBase<EstadisticaBarbero>, IEstadisticaBarberoRepository
{
    public EstadisticaBarberoRepository(ApiDbContext context) : base(context) { }

    public async Task<IEnumerable<EstadisticaBarbero>> ObtenerPorBarberoAsync(Guid barberoId)
    {
        return await _dbSet.Where(e => e.BarberoId == barberoId).OrderByDescending(e => e.Fecha).ToListAsync();
    }

    public async Task<EstadisticaBarbero?> ObtenerUltimaAsync(Guid barberoId)
    {
        return await _dbSet
            .Where(e => e.BarberoId == barberoId)
            .OrderByDescending(e => e.Fecha)
            .FirstOrDefaultAsync();
    }
}
