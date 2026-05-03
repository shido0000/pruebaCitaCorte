using API.Data.Entidades.Multibarbero;

namespace API.Data.IUnitOfWorks.Interfaces.Multibarbero
{
    public interface IAfiliacionBarberoRepository : IBaseRepository<AfiliacionBarbero>
    {
        Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarberoAsync(Guid barberoId);
        Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarberiaAsync(Guid barberiaId);
        Task<AfiliacionBarbero?> ObtenerAfiliacionActivaPorBarberoAsync(Guid barberoId);
    }
}
