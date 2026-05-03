using API.Data.DbContexts;
using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces.Multibarbero;

namespace API.Data.IUnitOfWorks.Repositorios.Multibarbero
{
    public class SolicitudAfiliacionRepository : BaseRepository<SolicitudAfiliacion>, ISolicitudAfiliacionRepository
    {
        public SolicitudAfiliacionRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
