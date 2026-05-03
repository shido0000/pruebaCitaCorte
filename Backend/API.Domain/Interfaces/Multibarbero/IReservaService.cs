using API.Data.Entidades.Multibarbero;
using API.Domain.Validators.Multibarbero;

namespace API.Domain.Interfaces.Multibarbero
{
    public interface IReservaService : IBaseService<Reserva, ReservaValidator>
    {
        Task<IEnumerable<Reserva>> ObtenerReservasPorCliente(Guid clienteId);
        Task<IEnumerable<Reserva>> ObtenerReservasPorProveedor(Guid proveedorId, API.Data.Enum.Multibarbero.TipoProveedor tipoProveedor);
        Task<IEnumerable<Reserva>> ObtenerReservasPorFecha(DateTime fecha, Guid? proveedorId = null, API.Data.Enum.Multibarbero.TipoProveedor? tipoProveedor = null);
        Task<Reserva?> ConfirmarReserva(Guid reservaId);
        Task<Reserva?> CancelarReserva(Guid reservaId, string motivo);
        Task<Reserva?> RechazarReserva(Guid reservaId, string motivo);
    }
}
