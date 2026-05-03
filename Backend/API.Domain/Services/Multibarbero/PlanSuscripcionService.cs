using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Multibarbero
{
    public class PlanSuscripcionService : BasicService<PlanSuscripcion, PlanSuscripcionValidator>, IPlanSuscripcionService
    {
        public PlanSuscripcionService(IUnitOfWork<PlanSuscripcion> repositorios, IHttpContextAccessor httpContext)
            : base(repositorios, httpContext)
        {
        }

        public async Task<IEnumerable<PlanSuscripcion>> ObtenerPlanesActivos(API.Data.Enum.Multibarbero.TipoPlan? tipo = null)
        {
            IQueryable<PlanSuscripcion> query = _repositorios.BasicRepository.GetQuery();

            query = query.Where(p => p.Activo);

            if (tipo.HasValue)
            {
                query = query.Where(p => p.Tipo == tipo.Value);
            }

            return await query.OrderByDescending(p => p.Precio).ToListAsync();
        }

        public async Task<PlanSuscripcion?> ObtenerPlanPorId(Guid id, bool incluirCaracteristicas = false)
        {
            Func<IQueryable<PlanSuscripcion>, IIncludableQueryable<PlanSuscripcion, object>>? includeFunc = null;

            if (incluirCaracteristicas)
            {
                includeFunc = q => q.Include(p => p.Caracteristicas);
            }

            return await _repositorios.BasicRepository.FirstAsync(p => p.Id == id, includeFunc);
        }
    }
}
