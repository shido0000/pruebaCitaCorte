using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;
using API.Data.IUnitOfWorks.Repositorios;

namespace API.Data.Repositories.Multibarbero;

public class CategoriaServicioRepository : BaseRepository<CategoriaServicio>, ICategoriaServicioRepository
{
    public CategoriaServicioRepository(ApiDbContext context) : base(context)
    {
    }
}
