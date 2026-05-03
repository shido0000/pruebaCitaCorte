using API.Data.Entidades.Nomencladores;
using API.Domain.Validators.Nomencladores;

namespace API.Domain.Interfaces.Nomencladores
{
    public interface IOrigenService : IBaseService<Origen, OrigenValidator>
    {
    }
}