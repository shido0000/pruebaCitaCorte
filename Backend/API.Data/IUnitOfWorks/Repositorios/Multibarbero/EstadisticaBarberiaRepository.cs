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

public class EstadisticaBarberiaRepository : RepositoryBase<EstadisticaBarberia>, IEstadisticaBarberiaRepository
{
    public EstadisticaBarberiaRepository(ApiDbContext context) : base(context) { }

    public async Task<IEnumerable<EstadisticaBarberia>> ObtenerPorBarberiaAsync(Guid barberiaId)
    {
        return await _dbSet.Where(e => e.BarberiaId == barberiaId).OrderByDescending(e => e.Fecha).ToListAsync();
    }

    public async Task<EstadisticaBarberia?> ObtenerUltimaAsync(Guid barberiaId)
    {
        return await _dbSet
            .Where(e => e.BarberiaId == barberiaId)
            .OrderByDescending(e => e.Fecha)
            .FirstOrDefaultAsync();
    }
}
