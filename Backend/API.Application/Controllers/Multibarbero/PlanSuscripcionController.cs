using API.Application.Dtos.Multibarbero.PlanSuscripcion;
using API.Data.Entidades.Multibarbero;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using AutoMapper;
using System.Linq.Expressions;

namespace API.Application.Controllers.Multibarbero
{
    public class PlanSuscripcionController : BasicController<PlanSuscripcion, PlanSuscripcionValidator, DetallesPlanSuscripcionDto, CrearPlanSuscripcionInputDto, ActualizarPlanSuscripcionInputDto, ListadoPaginadoPlanSuscripcionDto, FiltrarConfigurarListadoPaginadoPlanSuscripcionInputDto>
    {
        private readonly IPlanSuscripcionService _planSuscripcionService;

        public PlanSuscripcionController(IMapper mapper, IPlanSuscripcionService servicioPlanSuscripcion) : base(mapper, servicioPlanSuscripcion)
        {
            _planSuscripcionService = servicioPlanSuscripcion;
        }

        protected override Task<(IEnumerable<PlanSuscripcion>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoPlanSuscripcionInputDto inputDto)
        {
            List<Expression<Func<PlanSuscripcion, bool>>> filtros = new();
            
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(p => p.Nombre.Contains(inputDto.TextoBuscar) || 
                                 p.Descripcion.Contains(inputDto.TextoBuscar));
            }

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

        [HttpGet("[action]/activos")]
        public async Task<IActionResult> ObtenerPlanesActivos([FromQuery] API.Data.Enum.Multibarbero.TipoPlan? tipo = null)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");
            
            var planes = await _planSuscripcionService.ObtenerPlanesActivos(tipo);
            var planesDto = _mapper.Map<IEnumerable<DetallesPlanSuscripcionDto>>(planes);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = planesDto });
        }

        [HttpGet("[action]/por-id/{id}")]
        public async Task<IActionResult> ObtenerPlanPorId(Guid id, [FromQuery] bool incluirCaracteristicas = false)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");
            
            var plan = await _planSuscripcionService.ObtenerPlanPorId(id, incluirCaracteristicas);
            
            if (plan == null)
                return NotFound(new ResponseDto { Status = StatusCodes.Status404NotFound, ErrorMessage = "Plan no encontrado" });

            var planDto = _mapper.Map<DetallesPlanSuscripcionDto>(plan);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = planDto });
        }
    }
}
