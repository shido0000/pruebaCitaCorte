using API.Data.Context;
using API.Data.Repositories.Base;
using API.Domain.Entities.Multibarbero;
using API.Domain.Interfaces.Repositories.Multibarbero;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero;

public class CategoriaServicioRepository : RepositoryBase<CategoriaServicio>, ICategoriaServicioRepository
{
    public CategoriaServicioRepository(ApiDbContext context) : base(context) { }
}
