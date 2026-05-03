using API.Data.Entidades.Multibarbero;
using AutoMapper;

namespace API.Application.Mapper.Multibarbero;

public class PlanSuscripcionDtoProfile : Profile
{
    public PlanSuscripcionDtoProfile()
    {
        // Entidad a DTO
        CreateMap<PlanSuscripcion, PlanSuscripcionDto>()
            .ForMember(dest => dest.TipoPlan, opt => opt.MapFrom(src => src.Tipo));

        // DTO a Entidad (para actualizaciones)
        CreateMap<ActualizarPlanSuscripcionInputDto, PlanSuscripcion>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Crear InputDto a Entidad
        CreateMap<CrearPlanSuscripcionInputDto, PlanSuscripcion>();

        // Para listado paginado
        CreateMap<PlanSuscripcion, ListadoPaginadoPlanSuscripcionDto>()
            .ForMember(dest => dest.TipoPlan, opt => opt.MapFrom(src => src.Tipo));

        // Detalles
        CreateMap<PlanSuscripcion, DetallesPlanSuscripcionDto>()
            .ForMember(dest => dest.TipoPlan, opt => opt.MapFrom(src => src.Tipo))
            .ForMember(dest => dest.Caracteristicas, opt => opt.MapFrom(src => src.Caracteristicas))
            .ForMember(dest => dest.Limites, opt => opt.MapFrom(src => src.Limites));
    }
}
