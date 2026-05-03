using API.Data.Context;
using API.Data.Repositories.Base;
using API.Domain.Entities.Multibarbero;
using API.Domain.Interfaces.Repositories.Multibarbero;

namespace API.Data.Repositories.Multibarbero;

public class ServicioRepository : RepositoryBase<Servicio>, IServicioRepository
{
    public ServicioRepository(ApiDbContext context) : base(context)
    {
    }
}
