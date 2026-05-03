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

public class AfiliacionBarberoRepository : RepositoryBase<AfiliacionBarbero>, IAfiliacionBarberoRepository
{
    public AfiliacionBarberoRepository(ApiDbContext context) : base(context) { }

    public async Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarberoAsync(Guid barberoId)
    {
        return await _dbSet.Where(ab => ab.BarberoId == barberoId).ToListAsync();
    }

    public async Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarberiaAsync(Guid barberiaId)
    {
        return await _dbSet.Where(ab => ab.BarberiaId == barberiaId).ToListAsync();
    }

    public async Task<AfiliacionBarbero?> ObtenerPorBarberoYBarberiaAsync(Guid barberoId, Guid barberiaId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ab => ab.BarberoId == barberoId && ab.BarberiaId == barberiaId);
    }

    public async Task<int> ObtenerTotalAfiliacionesPorBarberoAsync(Guid barberoId)
    {
        return await _dbSet.CountAsync(ab => ab.BarberoId == barberoId && ab.Estado);
    }
}
