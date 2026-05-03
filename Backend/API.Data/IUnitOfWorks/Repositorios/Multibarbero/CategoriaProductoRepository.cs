using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero;

public class CategoriaProductoRepository : BaseRepository<CategoriaProducto>, ICategoriaProductoRepository
{
    public CategoriaProductoRepository(ApiDbContext context) : base(context) { }
}
