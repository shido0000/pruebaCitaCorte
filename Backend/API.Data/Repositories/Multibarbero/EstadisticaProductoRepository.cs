using API.Data.Context;
using API.Data.Repositories.Base;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Data.Repositories.Multibarbero;

public class EstadisticaProductoRepository : RepositoryBase<EstadisticaProducto>, IEstadisticaProductoRepository
{
    public EstadisticaProductoRepository(ApiDbContext context) : base(context)
    {
    }
}
