using API.Application.Dtos.Comunes;
using API.Application.Dtos.Multibarbero.Notificacion;
using API.Data.Entidades.Multibarbero;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Multibarbero
{
    public class NotificacionController : BasicController<Notificacion, NotificacionValidator, DetallesNotificacionDto, CrearNotificacionInputDto, ActualizarNotificacionInputDto, ListadoPaginadoNotificacionDto, FiltrarConfigurarListadoPaginadoNotificacionInputDto>
    {
        private readonly INotificacionService _notificacionService;

        public NotificacionController(IMapper mapper, INotificacionService servicioNotificacion) : base(mapper, servicioNotificacion)
        {
            _notificacionService = servicioNotificacion;
        }

        protected override Task<(IEnumerable<Notificacion>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoNotificacionInputDto inputDto)
        {
            List<Expression<Func<Notificacion, bool>>> filtros = new();

            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(n => n.Titulo.Contains(inputDto.TextoBuscar) ||
                                 n.Mensaje.Contains(inputDto.TextoBuscar));
            }

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

        [HttpGet("[action]/por-usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerNotificacionesPorUsuario(Guid usuarioId, [FromQuery] bool? leidas = null)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");

            var notificaciones = await _notificacionService.ObtenerNotificacionesPorUsuario(usuarioId, leidas);
            var notificacionesDto = _mapper.Map<IEnumerable<DetallesNotificacionDto>>(notificaciones);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = notificacionesDto });
        }

        [HttpPost("[action]/marcar-leida/{notificacionId}")]
        public async Task<IActionResult> MarcarComoLeida(Guid notificacionId)
        {
            _servicioBase.ValidarPermisos("gestionar");

            await _notificacionService.MarcarComoLeida(notificacionId);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Message = "Notificación marcada como leída." });
        }

        [HttpPost("[action]/marcar-todas-leidas/{usuarioId}")]
        public async Task<IActionResult> MarcarTodasComoLeidas(Guid usuarioId)
        {
            _servicioBase.ValidarPermisos("gestionar");

            await _notificacionService.MarcarTodasComoLeidas(usuarioId);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Message = "Todas las notificaciones marcadas como leídas." });
        }

        [HttpPost("[action]/crear")]
        public async Task<IActionResult> CrearNotificacion([FromBody] CrearNotificacionInputDto inputDto)
        {
            _servicioBase.ValidarPermisos("gestionar");

            var notificacion = _mapper.Map<Notificacion>(inputDto);
            var notificacionCreada = await _notificacionService.CrearNotificacion(notificacion);
            var notificacionDto = _mapper.Map<DetallesNotificacionDto>(notificacionCreada);

            return Ok(new ResponseDto { Status = StatusCodes.Status201Created, Result = notificacionDto });
        }
    }
}
