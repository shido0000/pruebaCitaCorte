using API.Application.Dtos.Multibarbero.Producto;
using API.Data.Entidades.Multibarbero;
using AutoMapper;

namespace API.Application.Mapper.Multibarbero
{
    public class ProductoDtoProfile : Profile
    {
        public ProductoDtoProfile()
        {
            CreateMap<Producto, ProductoDto>()
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : string.Empty))
                .ForMember(dest => dest.BarberoNombre, opt => opt.Ignore());

            CreateMap<Producto, ProductoDetallesDto>()
                .IncludeBase<Producto, ProductoDto>()
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : string.Empty))
                .ForMember(dest => dest.BarberoNombre, opt => opt.Ignore())
                .ForMember(dest => dest.HistorialVentasJson, opt => opt.Ignore());

            CreateMap<CrearProductoDto, Producto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaActualizacion, opt => opt.Ignore());

            CreateMap<ActualizarProductoDto, Producto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BarberoId, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore());
        }
    }
}
