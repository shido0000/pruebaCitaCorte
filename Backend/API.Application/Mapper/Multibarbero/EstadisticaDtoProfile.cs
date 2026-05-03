using API.Application.Dtos.Multibarbero.Estadisticas;
using AutoMapper;

namespace API.Application.Mapper.Multibarbero
{
    /// <summary>
    /// Perfil de mapeo para Estadisticas y sus DTOs relacionados
    /// </summary>
    public class EstadisticaDtoProfile : Profile
    {
        public EstadisticaDtoProfile()
        {
            // Mapeo de EstadisticasBarbero a EstadisticaBarberoDto
            CreateMap<EstadisticasBarbero, EstadisticaBarberoDto>()
                .ForMember(dest => dest.BarberoNombre, opt => opt.MapFrom(src => src.PerfilBarbero != null ? src.PerfilBarbero.Usuario.NombreCompleto : null));

            // Mapeo de EstadisticasBarberia a EstadisticaBarberiaDto
            CreateMap<EstadisticasBarberia, EstadisticaBarberiaDto>()
                .ForMember(dest => dest.BarberiaNombre, opt => opt.MapFrom(src => src.PerfilBarberia != null ? src.PerfilBarberia.NombreComercial : null));

            // Mapeo de EstadisticasProducto a EstadisticaProductoDto
            CreateMap<EstadisticasProducto, EstadisticaProductoDto>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.Producto != null ? src.Producto.Nombre : null));
        }
    }
}
