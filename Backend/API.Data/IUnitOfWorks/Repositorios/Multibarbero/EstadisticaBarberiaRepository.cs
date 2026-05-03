using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero;

public class EstadisticaBarberiaRepository : BaseRepository<EstadisticaBarberia>, IEstadisticaBarberiaRepository
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
