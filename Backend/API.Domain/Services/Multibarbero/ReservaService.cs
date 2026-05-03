using API.Data.Entidades.Multibarbero;
using API.Data.IUnitOfWorks.Interfaces;
using API.Domain.Exceptions;
using API.Domain.Interfaces.Multibarbero;
using API.Domain.Validators.Multibarbero;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Services.Multibarbero
{
    public class ReservaService : BasicService<Reserva, ReservaValidator>, IReservaService
    {
        public ReservaService(IUnitOfWork<Reserva> repositorios, IHttpContextAccessor httpContext)
            : base(repositorios, httpContext)
        {
        }

        public async Task<IEnumerable<Reserva>> ObtenerReservasPorCliente(Guid clienteId)
        {
            return await _repositorios.BasicRepository.GetQuery()
                .Where(r => r.ClienteId == clienteId)
                .Include(r => r.Servicio)
                .OrderByDescending(r => r.FechaHoraInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> ObtenerReservasPorProveedor(Guid proveedorId, API.Data.Enum.Multibarbero.TipoProveedor tipoProveedor)
        {
            return await _repositorios.BasicRepository.GetQuery()
                .Where(r => r.ProveedorId == proveedorId && r.TipoProveedor == tipoProveedor)
                .Include(r => r.Servicio)
                .Include(r => r.Cliente)
                .OrderByDescending(r => r.FechaHoraInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> ObtenerReservasPorFecha(DateTime fecha, Guid? proveedorId = null, API.Data.Enum.Multibarbero.TipoProveedor? tipoProveedor = null)
        {
            IQueryable<Reserva> query = _repositorios.BasicRepository.GetQuery()
                .Where(r => r.FechaHoraInicio.Date == fecha.Date);

            if (proveedorId.HasValue)
            {
                query = query.Where(r => r.ProveedorId == proveedorId.Value);
            }

            if (tipoProveedor.HasValue)
            {
                query = query.Where(r => r.TipoProveedor == tipoProveedor.Value);
            }

            return await query
                .Include(r => r.Servicio)
                .Include(r => r.Cliente)
                .OrderBy(r => r.FechaHoraInicio)
                .ToListAsync();
        }

        public async Task<Reserva?> ConfirmarReserva(Guid reservaId)
        {
            var reserva = await ObtenerPorId(reservaId) ??
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Reserva no encontrada." };

            if (reserva.Estado != API.Data.Enum.Multibarbero.EstadoReserva.Pendiente)
            {
                throw new CustomException { Status = StatusCodes.Status400BadRequest, Message = "La reserva ya ha sido procesada." };
            }

            reserva.Estado = API.Data.Enum.Multibarbero.EstadoReserva.Confirmada;
            reserva.FechaConfirmacion = DateTime.Now;

            await Actualizar(reserva);
            await SalvarCambios();

            return reserva;
        }

        public async Task<Reserva?> CancelarReserva(Guid reservaId, string motivo)
        {
            var reserva = await ObtenerPorId(reservaId) ??
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Reserva no encontrada." };

            if (reserva.Estado == API.Data.Enum.Multibarbero.EstadoReserva.Cancelada)
            {
                throw new CustomException { Status = StatusCodes.Status400BadRequest, Message = "La reserva ya está cancelada." };
            }

            reserva.Estado = API.Data.Enum.Multibarbero.EstadoReserva.Cancelada;
            reserva.FechaCancelacion = DateTime.Now;
            reserva.NotasProveedor = motivo;

            await Actualizar(reserva);
            await SalvarCambios();

            return reserva;
        }

        public async Task<Reserva?> RechazarReserva(Guid reservaId, string motivo)
        {
            var reserva = await ObtenerPorId(reservaId) ??
                throw new CustomException { Status = StatusCodes.Status404NotFound, Message = "Reserva no encontrada." };

            if (reserva.Estado != API.Data.Enum.Multibarbero.EstadoReserva.Pendiente)
            {
                throw new CustomException { Status = StatusCodes.Status400BadRequest, Message = "Solo se pueden rechazar reservas pendientes." };
            }

            reserva.Estado = API.Data.Enum.Multibarbero.EstadoReserva.Rechazada;
            reserva.FechaCancelacion = DateTime.Now;
            reserva.MotivoRechazo = motivo;

            await Actualizar(reserva);
            await SalvarCambios();

            return reserva;
        }
    }
}
