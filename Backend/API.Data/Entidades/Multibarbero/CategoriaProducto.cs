namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Categoría de productos (productos para el cabello, aceites, etc.)
    /// </summary>
    public class CategoriaProducto : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        public ICollection<Producto>? Productos { get; set; } = new List<Producto>();
    }
}
