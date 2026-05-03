using API.Data.Context;
using API.Data.Repositories.Base;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Data.Repositories.Multibarbero;

public class CategoriaServicioRepository : RepositoryBase<CategoriaServicio>, ICategoriaServicioRepository
{
    public CategoriaServicioRepository(ApiDbContext context) : base(context)
    {
    }
}
