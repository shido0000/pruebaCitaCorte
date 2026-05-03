using API.Data.Entidades.Multibarbero;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Data.IUnitOfWorks.Interfaces.Multibarbero
{
    public interface IServicioRepository : IBaseRepository<Servicio>
    {
        Task<IEnumerable<Servicio>> ObtenerPorBarberiaAsync(Guid idBarberia);
        Task<IEnumerable<Servicio>> ObtenerPorBarberiasAsync(IEnumerable<Guid> idBarberias);
        Task<IEnumerable<Servicio>> ObtenerPorCategoriaAsync(Guid idCategoria);
        Task<IEnumerable<Servicio>> ObtenerPorCategoriaYBarberiasAsync(Guid idCategoria, IEnumerable<Guid> idBarberias);
        Task<bool> ExisteNombreEnBarberiaAsync(string nombre, Guid idBarberia, Guid? idExcluir = null);
    }
}
