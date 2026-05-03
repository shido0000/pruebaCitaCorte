using API.Data.Entidades.Multibarbero;
using AutoMapper;

namespace API.Application.Mapper.Multibarbero;

public class NotificacionDtoProfile : Profile
{
    public NotificacionDtoProfile()
    {
        // Entidad a DTO
        CreateMap<Notificacion, NotificacionDto>()
            .ForMember(dest => dest.TipoTexto, opt => opt.MapFrom(src => src.Tipo.ToString()))
            .ForMember(dest => dest.Leida, opt => opt.MapFrom(src => src.Leida));

        // DTO a Entidad (para actualizaciones)
        CreateMap<ActualizarNotificacionInputDto, Notificacion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Crear InputDto a Entidad
        CreateMap<CrearNotificacionInputDto, Notificacion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Leida, opt => opt.MapFrom(_ => false));

        // Para listado paginado
        CreateMap<Notificacion, ListadoPaginadoNotificacionDto>()
            .ForMember(dest => dest.TipoTexto, opt => opt.MapFrom(src => src.Tipo.ToString()));

        // Detalles
        CreateMap<Notificacion, DetallesNotificacionDto>()
            .ForMember(dest => dest.TipoTexto, opt => opt.MapFrom(src => src.Tipo.ToString()))
            .ForMember(dest => dest.NombreDestinatario, opt => opt.MapFrom(src => src.UsuarioDestino.Usuario.Nombre));
    }
}
