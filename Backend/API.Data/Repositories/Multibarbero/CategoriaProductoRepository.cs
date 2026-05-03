using API.Data.Context;
using API.Data.Repositories.Base;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Data.Repositories.Multibarbero;

public class CategoriaProductoRepository : RepositoryBase<CategoriaProducto>, ICategoriaProductoRepository
{
    public CategoriaProductoRepository(ApiDbContext context) : base(context)
    {
    }
}
