using AutoMapper;
using API.Application.Dtos.Multibarbero;
using API.Data.Entidades.Multibarbero;

namespace API.Application.Mapper.Multibarbero
{
    /// <summary>
    /// Perfil de mapeo para Servicio y sus DTOs relacionados
    /// </summary>
    public class ServicioDtoProfile : Profile
    {
        public ServicioDtoProfile()
        {
            // Mapeo de Servicio a ServicioDto
            CreateMap<Servicio, ServicioDto>()
                .ForMember(dest => dest.CategoriaServicioNombre, opt => opt.MapFrom(src => src.CategoriaServicio != null ? src.CategoriaServicio.Nombre : null))
                .ForMember(dest => dest.BarberiaNombre, opt => opt.MapFrom(src => src.PerfilBarberia != null ? src.PerfilBarberia.NombreComercial : null));

            // Mapeo de CrearServicioDto (asumiendo que existe) a Servicio
            // Si no existe el DTO de creación, se puede usar el mismo ServicioDto con Ignore para campos automáticos
            CreateMap<ServicioDto, Servicio>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CategoriaServicio, opt => opt.Ignore())
                .ForMember(dest => dest.PerfilBarberia, opt => opt.Ignore())
                .ForMember(dest => dest.Reservas, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore());
        }
    }
}
