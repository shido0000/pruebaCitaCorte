using API.Data.Context;
using API.Data.Repositories.Base;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Data.Repositories.Multibarbero;

public class EstadisticaBarberiaRepository : RepositoryBase<EstadisticaBarberia>, IEstadisticaBarberiaRepository
{
    public EstadisticaBarberiaRepository(ApiDbContext context) : base(context)
    {
    }
}
