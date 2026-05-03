using API.Application.Dtos.Comunes;
using API.Application.Dtos.Multibarbero.PerfilBarbero;
using API.Data.Entidades.Multibarbero;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Application.Controllers.Multibarbero
{
    public class PerfilBarberoController : BasicController<PerfilBarbero, PerfilBarberoValidator, DetallesPerfilBarberoDto, CrearPerfilBarberoInputDto, ActualizarPerfilBarberoInputDto, ListadoPaginadoPerfilBarberoDto, FiltrarConfigurarListadoPaginadoPerfilBarberoInputDto>
    {
        private readonly IPerfilBarberoService _perfilBarberoService;

        public PerfilBarberoController(IMapper mapper, IPerfilBarberoService servicioPerfilBarbero) : base(mapper, servicioPerfilBarbero)
        {
            _perfilBarberoService = servicioPerfilBarbero;
        }

        protected override Task<(IEnumerable<PerfilBarbero>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoPerfilBarberoInputDto inputDto)
        {
            List<Expression<Func<PerfilBarbero, bool>>> filtros = new();

            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(b => b.Nombre.Contains(inputDto.TextoBuscar) ||
                                 b.Apellidos.Contains(inputDto.TextoBuscar) ||
                                 (b.Email != null && b.Email.Contains(inputDto.TextoBuscar)));
            }

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

        [HttpGet("[action]/por-barberia/{barberiaId}")]
        public async Task<IActionResult> ObtenerBarberosPorBarberia(Guid barberiaId)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");

            var barberos = await _perfilBarberoService.ObtenerBarberosPorBarberia(barberiaId);
            var barberosDto = _mapper.Map<IEnumerable<DetallesPerfilBarberoDto>>(barberos);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = barberosDto });
        }

        [HttpGet("[action]/por-usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerBarberoPorUsuarioId(Guid usuarioId)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");

            var barbero = await _perfilBarberoService.ObtenerBarberoPorUsuarioId(usuarioId);

            if (barbero == null)
                return NotFound(new ResponseDto { Status = StatusCodes.Status404NotFound, ErrorMessage = "Barbero no encontrado" });

            var barberoDto = _mapper.Map<DetallesPerfilBarberoDto>(barbero);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = barberoDto });
        }

        [HttpGet("[action]/es-principal/{barberoId}")]
        public async Task<IActionResult> EsBarberoPrincipal(Guid barberoId)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");

            var esPrincipal = await _perfilBarberoService.EsBarberoPrincipal(barberoId);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = esPrincipal });
        }
    }
}
