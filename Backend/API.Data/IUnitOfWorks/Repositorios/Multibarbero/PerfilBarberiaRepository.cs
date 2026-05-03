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

public class PerfilBarberiaRepository : RepositoryBase<PerfilBarberia>, IPerfilBarberiaRepository
{
    public PerfilBarberiaRepository(ApiDbContext context) : base(context) { }

    public async Task<PerfilBarberia?> ObtenerPorBarberiaAsync(Guid barberiaId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(pb => pb.BarberiaId == barberiaId);
    }

    public async Task<IEnumerable<PerfilBarberia>> ObtenerPorBarberoAsync(Guid barberoId)
    {
        return await _context.AfiliacionesBarbero
            .Where(ab => ab.BarberoId == barberoId && ab.Estado)
            .Join(_context.PerfilBarberias,
                ab => ab.BarberiaId,
                pb => pb.BarberiaId,
                (ab, pb) => pb)
            .ToListAsync();
    }

    public async Task<PerfilBarberia?> ObtenerPorBarberoYBarberiaAsync(Guid barberoId, Guid barberiaId)
    {
        var afiliacion = await _context.AfiliacionesBarbero
            .FirstOrDefaultAsync(ab => ab.BarberoId == barberoId && ab.BarberiaId == barberiaId && ab.Estado);

        if (afiliacion == null) return null;

        return await _dbSet.FirstOrDefaultAsync(pb => pb.BarberiaId == barberiaId);
    }
}
