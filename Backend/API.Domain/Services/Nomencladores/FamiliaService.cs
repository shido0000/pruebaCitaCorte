using API.Data.Entidades.Nomencladores;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Nomencladores;
using API.Domain.Validators.Nomencladores;
using Microsoft.AspNetCore.Http;

namespace API.Domain.Services.Nomencladores
{
    public class FamiliaService : BasicService<Familia, FamiliaValidator>, IFamiliaService
    {

        public FamiliaService(IUnitOfWork<Familia> repositorios, IHttpContextAccessor httpContext) : base(repositorios, httpContext)
        {
        }


    }
}