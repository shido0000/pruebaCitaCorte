using API.Application.Dtos.Comunes;
using API.Application.Dtos.Multibarbero.Afiliacion;
using API.Application.Dtos.Multibarbero.PlanSuscripcion;
using API.Application.Dtos.Multibarbero.Reserva;
using API.Data.Entidades.Multibarbero;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Multibarbero
{
    public class SolicitudAfiliacionController : BasicController<SolicitudAfiliacion, SolicitudAfiliacionValidator, DetallesSolicitudAfiliacionDto, CrearSolicitudAfiliacionInputDto, ActualizarSolicitudAfiliacionInputDto, ListadoPaginadoReservaDto, FiltrarConfigurarListadoPaginadoPlanSuscripcionInputDto>
    {
        private readonly ISolicitudAfiliacionService _solicitudAfiliacionService;

        public SolicitudAfiliacionController(IMapper mapper, ISolicitudAfiliacionService servicioSolicitudAfiliacion) : base(mapper, servicioSolicitudAfiliacion)
        {
            _solicitudAfiliacionService = servicioSolicitudAfiliacion;
        }

        protected override Task<(IEnumerable<SolicitudAfiliacion>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoPlanSuscripcionInputDto inputDto)
        {
            List<Expression<Func<SolicitudAfiliacion, bool>>> filtros = new();

            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(s => s.Motivo.Contains(inputDto.TextoBuscar));
            }

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

        [HttpGet("[action]/por-barbero/{barberoId}")]
        public async Task<IActionResult> ObtenerSolicitudesPorBarbero(Guid barberoId)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");

            var solicitudes = await _solicitudAfiliacionService.ObtenerSolicitudesPorBarbero(barberoId);
            var solicitudesDto = _mapper.Map<IEnumerable<DetallesSolicitudAfiliacionDto>>(solicitudes);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = solicitudesDto });
        }

        [HttpGet("[action]/por-barberia/{barberiaId}")]
        public async Task<IActionResult> ObtenerSolicitudesPorBarberia(Guid barberiaId)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");

            var solicitudes = await _solicitudAfiliacionService.ObtenerSolicitudesPorBarberia(barberiaId);
            var solicitudesDto = _mapper.Map<IEnumerable<DetallesSolicitudAfiliacionDto>>(solicitudes);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = solicitudesDto });
        }

        [HttpPost("[action]/aprobar/{solicitudId}")]
        public async Task<IActionResult> AprobarSolicitud(Guid solicitudId)
        {
            _servicioBase.ValidarPermisos("gestionar");

            var usuarioId = Guid.Parse(User?.FindSystemResourceClaim("sub")?.Value ?? throw new CustomException { Status = StatusCodes.Status401Unauthorized, Message = "Usuario no identificado." });
            var solicitud = await _solicitudAfiliacionService.AprobarSolicitud(solicitudId, usuarioId);
            var solicitudDto = _mapper.Map<DetallesSolicitudAfiliacionDto>(solicitud);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = solicitudDto });
        }

        [HttpPost("[action]/rechazar/{solicitudId}")]
        public async Task<IActionResult> RechazarSolicitud(Guid solicitudId, [FromBody] string motivo)
        {
            _servicioBase.ValidarPermisos("gestionar");

            var usuarioId = Guid.Parse(User?.FindSystemResourceClaim("sub")?.Value ?? throw new CustomException { Status = StatusCodes.Status401Unauthorized, Message = "Usuario no identificado." });
            var solicitud = await _solicitudAfiliacionService.RechazarSolicitud(solicitudId, motivo, usuarioId);
            var solicitudDto = _mapper.Map<DetallesSolicitudAfiliacionDto>(solicitud);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = solicitudDto });
        }
    }
}
