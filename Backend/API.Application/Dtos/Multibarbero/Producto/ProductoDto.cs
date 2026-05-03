namespace API.Application.Dtos.Multibarbero.Producto
{
    public class ProductoDto
    {
        public Guid Id { get; set; }
        public Guid? CategoriaProductoId { get; set; }
        public string? CategoriaProductoNombre { get; set; }
        public Guid PerfilBarberoId { get; set; }
        public string? PerfilBarberoNombre { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? ImagenUrl { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
