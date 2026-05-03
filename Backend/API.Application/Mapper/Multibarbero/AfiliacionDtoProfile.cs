using AutoMapper;
using API.Application.Dtos.Multibarbero.Afiliaciones;
using API.Data.Entidades.Multibarbero;

namespace API.Application.Mapper.Multibarbero
{
    /// <summary>
    /// Perfil de mapeo para AfiliacionBarbero y sus DTOs relacionados
    /// </summary>
    public class AfiliacionDtoProfile : Profile
    {
        public AfiliacionDtoProfile()
        {
            // Mapeo de AfiliacionBarbero a AfiliacionDto
            CreateMap<AfiliacionBarbero, AfiliacionDto>()
                .ForMember(dest => dest.BarberoNombre, opt => opt.MapFrom(src => src.PerfilBarbero != null ? src.PerfilBarbero.Usuario.NombreCompleto : null))
                .ForMember(dest => dest.BarberoFotoUrl, opt => opt.MapFrom(src => src.PerfilBarbero != null ? src.PerfilBarbero.FotoPerfil : null))
                .ForMember(dest => dest.BarberiaNombre, opt => opt.MapFrom(src => src.PerfilBarberia != null ? src.PerfilBarberia.NombreComercial : null));

            // Mapeo de AfiliacionBarbero a AfiliacionDetallesDto
            CreateMap<AfiliacionBarbero, AfiliacionDetallesDto>()
                .ForMember(dest => dest.BarberoNombre, opt => opt.MapFrom(src => src.PerfilBarbero != null ? src.PerfilBarbero.Usuario.NombreCompleto : null))
                .ForMember(dest => dest.BarberoEmail, opt => opt.MapFrom(src => src.PerfilBarbero != null ? src.PerfilBarbero.Usuario.Email : null))
                .ForMember(dest => dest.BarberoFotoUrl, opt => opt.MapFrom(src => src.PerfilBarbero != null ? src.PerfilBarbero.FotoPerfil : null))
                .ForMember(dest => dest.BarberiaNombre, opt => opt.MapFrom(src => src.PerfilBarberia != null ? src.PerfilBarberia.NombreComercial : null))
                .ForMember(dest => dest.BarberiaDireccion, opt => opt.MapFrom(src => src.PerfilBarberia != null ? src.PerfilBarberia.Direccion : null));

            // Mapeo de CrearAfiliacionDto a AfiliacionBarbero
            CreateMap<CrearAfiliacionDto, AfiliacionBarbero>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PerfilBarbero, opt => opt.Ignore())
                .ForMember(dest => dest.PerfilBarberia, opt => opt.Ignore())
                .ForMember(dest => dest.FechaAfiliacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaFinAfiliacion, opt => opt.Ignore())
                .ForMember(dest => dest.MotivoFin, opt => opt.Ignore());
        }
    }
}
