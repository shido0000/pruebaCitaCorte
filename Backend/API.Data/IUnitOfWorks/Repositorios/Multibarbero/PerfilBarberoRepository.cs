using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero
{
    public class PerfilBarberoRepository : BaseRepository<PerfilBarbero>, IPerfilBarberoRepository
    {
        public PerfilBarberoRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
