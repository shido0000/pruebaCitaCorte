using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using Microsoft.EntityFrameworkCore;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero
{
    public class AfiliacionBarberoRepository : BaseRepository<AfiliacionBarbero>, IAfiliacionBarberoRepository
    {
        public AfiliacionBarberoRepository(ApiDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarberoAsync(Guid barberoId)
        {
            return await _dbSet
                .Where(a => a.BarberoId == barberoId)
                .Include(a => a.Barberia)
                .ToListAsync();
        }

        public async Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarberiaAsync(Guid barberiaId)
        {
            return await _dbSet
                .Where(a => a.BarberiaId == barberiaId)
                .Include(a => a.Barbero)
                .ToListAsync();
        }

        public async Task<AfiliacionBarbero?> ObtenerAfiliacionActivaPorBarberoAsync(Guid barberoId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.BarberoId == barberoId && a.Activo);
        }
    }
}
