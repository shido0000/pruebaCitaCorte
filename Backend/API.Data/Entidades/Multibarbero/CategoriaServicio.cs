using API.Data.Entidades;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Categoría de servicios (corte, afeitado, etc.)
    /// </summary>
    public class CategoriaServicio : EntidadBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Icono { get; set; } = string.Empty;

        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
    }
}

