using API.Data.Entidades.Multibarbero;
using API.Domain.Validators.Multibarbero;

namespace API.Domain.Interfaces.Multibarbero
{
    public interface IPerfilBarberoService : IBaseService<PerfilBarbero, PerfilBarberoValidator>
    {
        Task<IEnumerable<PerfilBarbero>> ObtenerBarberosPorBarberia(Guid barberiaId);
        Task<PerfilBarbero?> ObtenerBarberoPorUsuarioId(Guid usuarioId);
        Task<bool> EsBarberoPrincipal(Guid barberoId);
    }
}
