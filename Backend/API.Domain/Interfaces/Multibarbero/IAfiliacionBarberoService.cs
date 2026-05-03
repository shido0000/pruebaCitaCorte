using API.Data.Entidades.Multibarbero;

namespace API.Domain.Interfaces.Multibarbero
{
    public interface IAfiliacionBarberoService : IBaseService<AfiliacionBarbero>
    {
        Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarbero(Guid barberoId);
        Task<IEnumerable<AfiliacionBarbero>> ObtenerPorBarberia(Guid barberiaId);
        Task<AfiliacionBarbero?> ObtenerAfiliacionActiva(Guid barberoId);
        Task<AfiliacionBarbero> CrearAfiliacion(AfiliacionBarbero afiliacion);
        Task DesactivarAfiliacion(Guid afiliacionId, string motivo);
    }
}
