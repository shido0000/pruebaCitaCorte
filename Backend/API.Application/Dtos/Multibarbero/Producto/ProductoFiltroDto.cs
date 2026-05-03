namespace API.Application.Dtos.Multibarbero.Producto
{
    public class ProductoFiltroDto
    {
        public Guid? CategoriaProductoId { get; set; }
        public Guid? PerfilBarberoId { get; set; }
        public string? Nombre { get; set; }
        public decimal? PrecioMinimo { get; set; }
        public decimal? PrecioMaximo { get; set; }
        public bool? SoloActivos { get; set; }
        public int Pagina { get; set; } = 1;
        public int CantidadPorPagina { get; set; } = 10;
        public string? OrdenarPor { get; set; }
        public bool OrdenAscendente { get; set; } = true;
    }
}
