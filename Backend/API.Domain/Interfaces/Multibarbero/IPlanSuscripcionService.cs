using API.Data.Entidades.Multibarbero;
using API.Domain.Validators.Multibarbero;

namespace API.Domain.Interfaces.Multibarbero
{
    public interface IPlanSuscripcionService : IBaseService<PlanSuscripcion, PlanSuscripcionValidator>
    {
        Task<IEnumerable<PlanSuscripcion>> ObtenerPlanesActivos(API.Data.Enum.Multibarbero.TipoPlan? tipo = null);
        Task<PlanSuscripcion?> ObtenerPlanPorId(Guid id, bool incluirCaracteristicas = false);
    }
}
