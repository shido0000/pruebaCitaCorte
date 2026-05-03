using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Producto en venta por barberos (solo plan Premium)
    /// </summary>
    public class Producto : EntidadBase
    {
        [ForeignKey(nameof(CategoriaProducto))]
        public Guid? CategoriaProductoId { get; set; }
        public CategoriaProducto? CategoriaProducto { get; set; }

        [ForeignKey(nameof(PerfilBarbero))]
        public Guid PerfilBarberoId { get; set; }
        public PerfilBarbero PerfilBarbero { get; set; } = null!;

        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? ImagenUrl { get; set; }
        public bool Activo { get; set; } = true;
    }
}
