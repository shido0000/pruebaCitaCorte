using API.Application.Dtos.Multibarbero.Reserva;
using API.Data.Entidades.Multibarbero;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using AutoMapper;
using System.Linq.Expressions;

namespace API.Application.Controllers.Multibarbero
{
    public class ReservaController : BasicController<Reserva, ReservaValidator, DetallesReservaDto, CrearReservaInputDto, ActualizarReservaInputDto, ListadoPaginadoReservaDto, FiltrarConfigurarListadoPaginadoReservaInputDto>
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IMapper mapper, IReservaService servicioReserva) : base(mapper, servicioReserva)
        {
            _reservaService = servicioReserva;
        }

        protected override Task<(IEnumerable<Reserva>, int)> AplicarFiltrosIncluirPropiedades(FiltrarConfigurarListadoPaginadoReservaInputDto inputDto)
        {
            List<Expression<Func<Reserva, bool>>> filtros = new();
            
            if (!string.IsNullOrEmpty(inputDto.TextoBuscar))
            {
                filtros.Add(r => r.Notas.Contains(inputDto.TextoBuscar));
            }

            return _servicioBase.ObtenerListadoPaginado(inputDto.CantidadIgnorar, inputDto.CantidadMostrar, inputDto.SecuenciaOrdenamiento, null, filtros.ToArray());
        }

        [HttpGet("[action]/por-cliente/{clienteId}")]
        public async Task<IActionResult> ObtenerReservasPorCliente(Guid clienteId)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");
            
            var reservas = await _reservaService.ObtenerReservasPorCliente(clienteId);
            var reservasDto = _mapper.Map<IEnumerable<DetallesReservaDto>>(reservas);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = reservasDto });
        }

        [HttpGet("[action]/por-proveedor/{proveedorId}")]
        public async Task<IActionResult> ObtenerReservasPorProveedor(Guid proveedorId, [FromQuery] API.Data.Enum.Multibarbero.TipoProveedor tipoProveedor)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");
            
            var reservas = await _reservaService.ObtenerReservasPorProveedor(proveedorId, tipoProveedor);
            var reservasDto = _mapper.Map<IEnumerable<DetallesReservaDto>>(reservas);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = reservasDto });
        }

        [HttpGet("[action]/por-fecha/{fecha}")]
        public async Task<IActionResult> ObtenerReservasPorFecha(DateTime fecha, [FromQuery] Guid? proveedorId = null, [FromQuery] API.Data.Enum.Multibarbero.TipoProveedor? tipoProveedor = null)
        {
            _servicioBase.ValidarPermisos("listar, gestionar");
            
            var reservas = await _reservaService.ObtenerReservasPorFecha(fecha, proveedorId, tipoProveedor);
            var reservasDto = _mapper.Map<IEnumerable<DetallesReservaDto>>(reservas);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = reservasDto });
        }

        [HttpPost("[action]/confirmar/{reservaId}")]
        public async Task<IActionResult> ConfirmarReserva(Guid reservaId)
        {
            _servicioBase.ValidarPermisos("gestionar");
            
            var reserva = await _reservaService.ConfirmarReserva(reservaId);
            var reservaDto = _mapper.Map<DetallesReservaDto>(reserva);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = reservaDto });
        }

        [HttpPost("[action]/cancelar/{reservaId}")]
        public async Task<IActionResult> CancelarReserva(Guid reservaId, [FromBody] string motivo)
        {
            _servicioBase.ValidarPermisos("gestionar");
            
            var reserva = await _reservaService.CancelarReserva(reservaId, motivo);
            var reservaDto = _mapper.Map<DetallesReservaDto>(reserva);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = reservaDto });
        }

        [HttpPost("[action]/rechazar/{reservaId}")]
        public async Task<IActionResult> RechazarReserva(Guid reservaId, [FromBody] string motivo)
        {
            _servicioBase.ValidarPermisos("gestionar");
            
            var reserva = await _reservaService.RechazarReserva(reservaId, motivo);
            var reservaDto = _mapper.Map<DetallesReservaDto>(reserva);

            return Ok(new ResponseDto { Status = StatusCodes.Status200OK, Result = reservaDto });
        }
    }
}
