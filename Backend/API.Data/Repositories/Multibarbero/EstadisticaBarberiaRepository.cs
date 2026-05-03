using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using API.Data.IUnitOfWorks.Repositorios;

namespace API.Data.Repositories.Multibarbero;

public class EstadisticaBarberiaRepository : BaseRepository<EstadisticaBarberia>, IEstadisticaBarberiaRepository
{
    public EstadisticaBarberiaRepository(ApiDbContext context) : base(context)
    {
    }
}
