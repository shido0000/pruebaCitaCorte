using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using API.Data.IUnitOfWorks.Repositorios;

namespace API.Data.Repositories.Multibarbero;

public class EstadisticaBarberoRepository : BaseRepository<EstadisticaBarbero>, IEstadisticaBarberoRepository
{
    public EstadisticaBarberoRepository(ApiDbContext context) : base(context)
    {
    }
}
