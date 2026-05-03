namespace API.Application.Dtos.Multibarbero.Producto
{
    public class CrearProductoDto
    {
        public Guid? CategoriaProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? ImagenUrl { get; set; }
        public bool Activo { get; set; } = true;
    }
}
