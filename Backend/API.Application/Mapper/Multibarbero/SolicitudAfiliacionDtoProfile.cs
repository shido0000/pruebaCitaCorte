using AutoMapper;
using API.Application.Dtos.Multibarbero;
using API.Data.Entidades.Multibarbero;

namespace API.Application.Mapper.Multibarbero;

public class SolicitudAfiliacionDtoProfile : Profile
{
    public SolicitudAfiliacionDtoProfile()
    {
        // Entidad a DTO
        CreateMap<SolicitudAfiliacion, SolicitudAfiliacionDto>()
            .ForMember(dest => dest.NombreBarbero, opt => opt.MapFrom(src => src.Barbero.Usuario.Nombre))
            .ForMember(dest => dest.NombreBarberia, opt => opt.MapFrom(src => src.Barberia.Usuario.NombreComercial))
            .ForMember(dest => dest.EstadoTexto, opt => opt.MapFrom(src => src.Estado.ToString()))
            .ForMember(dest => dest.RespondidoPorNombre, opt => opt.MapFrom(src => src.RespondidoPor != null ? src.RespondidoPor.Usuario.Nombre : null));
        
        // DTO a Entidad (para actualizaciones)
        CreateMap<ActualizarSolicitudAfiliacionInputDto, SolicitudAfiliacion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        // Crear InputDto a Entidad
        CreateMap<CrearSolicitudAfiliacionInputDto, SolicitudAfiliacion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FechaSolicitud, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(_ => Data.Enum.Multibarbero.EstadoSolicitudAfiliacion.Pendiente));
    }
}
