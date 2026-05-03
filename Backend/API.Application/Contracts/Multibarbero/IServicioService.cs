using API.Data.Entidades.Multibarbero;

namespace API.Application.Services.Multibarbero
{
    public interface IServicioService : IServiceBase<Servicio>
    {
        Task<IEnumerable<Servicio>> ObtenerPorBarberiaAsync(Guid idBarberia);
        Task<IEnumerable<Servicio>> ObtenerPorCategoriaAsync(Guid idCategoria);
        Task<bool> ExisteNombreEnBarberiaAsync(string nombre, Guid idBarberia, Guid? idExcluir = null);
    }
}
