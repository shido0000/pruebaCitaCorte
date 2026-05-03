using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero;

public class PerfilBarberiaRepository : BaseRepository<PerfilBarberia>, IPerfilBarberiaRepository
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
