using API.Data.Entidades.Multibarbero;
using AutoMapper;

namespace API.Application.Mapper.Multibarbero;

public class PerfilBarberoDtoProfile : Profile
{
    public PerfilBarberoDtoProfile()
    {
        // Entidad a DTO
        CreateMap<PerfilBarbero, PerfilBarberoDto>()
            .ForMember(dest => dest.Especialidades, opt => opt.MapFrom(src => src.Especialidades))
            .ForMember(dest => dest.PlanActual, opt => opt.MapFrom(src => src.PlanSuscripcion));

        // DTO a Entidad (para actualizaciones)
        CreateMap<ActualizarPerfilBarberoInputDto, PerfilBarbero>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioId, opt => opt.Ignore());

        // Crear InputDto a Entidad
        CreateMap<CrearPerfilBarberoInputDto, PerfilBarbero>();

        // Para listado paginado
        CreateMap<PerfilBarbero, ListadoPaginadoPerfilBarberoDto>()
            .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Usuario.Nombre))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario.Email))
            .ForMember(dest => dest.TipoPlan, opt => opt.MapFrom(src => src.PlanSuscripcion.Nombre));
    }
}
