using API.Data.Entidades.Multibarbero;
using AutoMapper;

namespace API.Application.Mapper.Multibarbero;

public class ReservaDtoProfile : Profile
{
    public ReservaDtoProfile()
    {
        // Entidad a DTO
        CreateMap<Reserva, ReservaDto>()
            .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Usuario.Nombre))
            .ForMember(dest => dest.NombreProveedor, opt => opt.MapFrom(src => src.ProveedorTipo == Data.Enum.Multibarbero.TipoProveedor.Barbero
                ? src.ProveedorBarbero.Usuario.Nombre
                : src.ProveedorBarberia.Usuario.NombreComercial))
            .ForMember(dest => dest.ServicioNombre, opt => opt.MapFrom(src => src.Servicio.Nombre))
            .ForMember(dest => dest.EstadoTexto, opt => opt.MapFrom(src => src.Estado.ToString()));

        // DTO a Entidad (para actualizaciones)
        CreateMap<ActualizarReservaInputDto, Reserva>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Crear InputDto a Entidad
        CreateMap<CrearReservaInputDto, Reserva>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FechaSolicitud, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(_ => Data.Enum.Multibarbero.EstadoReserva.Pendiente));

        // Para listado paginado
        CreateMap<Reserva, ListadoPaginadoReservaDto>()
            .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Usuario.Nombre))
            .ForMember(dest => dest.ServicioNombre, opt => opt.MapFrom(src => src.Servicio.Nombre))
            .ForMember(dest => dest.EstadoTexto, opt => opt.MapFrom(src => src.Estado.ToString()));

        // Detalles
        CreateMap<Reserva, DetallesReservaDto>()
            .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Usuario.Nombre))
            .ForMember(dest => dest.EmailCliente, opt => opt.MapFrom(src => src.Cliente.Usuario.Email))
            .ForMember(dest => dest.TelefonoCliente, opt => opt.MapFrom(src => src.Cliente.Usuario.Telefono))
            .ForMember(dest => dest.NombreProveedor, opt => opt.MapFrom(src => src.ProveedorTipo == Data.Enum.Multibarbero.TipoProveedor.Barbero
                ? src.ProveedorBarbero.Usuario.Nombre
                : src.ProveedorBarberia.Usuario.NombreComercial))
            .ForMember(dest => dest.ServicioNombre, opt => opt.MapFrom(src => src.Servicio.Nombre))
            .ForMember(dest => dest.ServicioDuracion, opt => opt.MapFrom(src => src.Servicio.DuracionMinutos))
            .ForMember(dest => dest.ServicioPrecio, opt => opt.MapFrom(src => src.Servicio.Precio))
            .ForMember(dest => dest.EstadoTexto, opt => opt.MapFrom(src => src.Estado.ToString()));
    }
}
