using API.Data.Entidades.Nomencladores;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Nomencladores;
using API.Domain.Validators.Nomencladores;
using Microsoft.AspNetCore.Http;

namespace API.Domain.Services.Nomencladores
{
    public class OrigenService : BasicService<Origen, OrigenValidator>, IOrigenService
    {

        public OrigenService(IUnitOfWork<Origen> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }


    }
}