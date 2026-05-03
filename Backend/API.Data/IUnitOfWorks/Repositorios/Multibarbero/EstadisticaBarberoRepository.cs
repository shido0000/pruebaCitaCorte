using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero;

public class EstadisticaBarberoRepository : BaseRepository<EstadisticaBarbero>, IEstadisticaBarberoRepository
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
