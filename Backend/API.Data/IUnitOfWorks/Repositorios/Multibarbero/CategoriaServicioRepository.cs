using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero;

public class CategoriaServicioRepository : BaseRepository<CategoriaServicio>, ICategoriaServicioRepository
{
    public CategoriaServicioRepository(ApiDbContext context) : base(context) { }
}
