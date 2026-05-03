using API.Application.Dtos.Multibarbero.PerfilBarberia;
using API.Data.Entidades.Multibarbero;
using AutoMapper;

namespace API.Application.Mapper.Multibarbero
{
    /// <summary>
    /// Perfil de mapeo para PerfilBarberia y sus DTOs relacionados
    /// </summary>
    public class PerfilBarberiaDtoProfile : Profile
    {
        public PerfilBarberiaDtoProfile()
        {
            // Mapeo de PerfilBarberia a PerfilBarberiaDto
            CreateMap<PerfilBarberia, PerfilBarberiaDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.Usuario.NombreCompleto))
                .ForMember(dest => dest.UsuarioEmail, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.PlanSuscripcionNombre, opt => opt.MapFrom(src => src.PlanSuscripcion != null ? src.PlanSuscripcion.Nombre : null))
                .ForMember(dest => dest.EstadoSolicitudCambioPlan, opt => opt.MapFrom(src => src.SolicitudCambioPlan != null ? src.SolicitudCambioPlan.Estado.ToString() : null))
                .ForMember(dest => dest.TotalBarberosAfiliados, opt => opt.MapFrom(src => src.Afiliaciones != null ? src.Afiliaciones.Count : 0));

            // Mapeo de CrearPerfilBarberiaDto a PerfilBarberia
            CreateMap<CrearPerfilBarberiaDto, PerfilBarberia>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.PlanSuscripcionId, opt => opt.Ignore())
                .ForMember(dest => dest.PlanSuscripcion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaInicioPlan, opt => opt.Ignore())
                .ForMember(dest => dest.FechaVencimientoPlan, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudCambioPlan, opt => opt.Ignore())
                .ForMember(dest => dest.Afiliaciones, opt => opt.Ignore())
                .ForMember(dest => dest.Servicios, opt => opt.Ignore())
                .ForMember(dest => dest.Reservas, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaActualizacion, opt => opt.Ignore());

            // Mapeo de ActualizarPerfilBarberiaDto a PerfilBarberia
            CreateMap<ActualizarPerfilBarberiaDto, PerfilBarberia>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.PlanSuscripcionId, opt => opt.Ignore())
                .ForMember(dest => dest.PlanSuscripcion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaInicioPlan, opt => opt.Ignore())
                .ForMember(dest => dest.FechaVencimientoPlan, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudCambioPlan, opt => opt.Ignore())
                .ForMember(dest => dest.Afiliaciones, opt => opt.Ignore())
                .ForMember(dest => dest.Servicios, opt => opt.Ignore())
                .ForMember(dest => dest.Reservas, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaActualizacion, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Mapeo de PerfilBarberia a PerfilBarberiaDetallesDto
            CreateMap<PerfilBarberia, PerfilBarberiaDetallesDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.Usuario.NombreCompleto))
                .ForMember(dest => dest.UsuarioEmail, opt => opt.MapFrom(src => src.Usuario.Email))
                .ForMember(dest => dest.PlanSuscripcionNombre, opt => opt.MapFrom(src => src.PlanSuscripcion != null ? src.PlanSuscripcion.Nombre : null))
                .ForMember(dest => dest.EstadoSolicitudCambioPlan, opt => opt.MapFrom(src => src.SolicitudCambioPlan != null ? src.SolicitudCambioPlan.Estado.ToString() : null))
                .ForMember(dest => dest.TotalBarberosAfiliados, opt => opt.MapFrom(src => src.Afiliaciones != null ? src.Afiliaciones.Count : 0))
                .ForMember(dest => dest.BarberosAfiliados, opt => opt.MapFrom(src => src.Afiliaciones != null ? src.Afiliaciones : null));
        }
    }
}
