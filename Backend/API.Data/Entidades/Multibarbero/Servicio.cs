using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Servicio ofrecido por barberos/barberías
    /// </summary>
    public class Servicio : EntidadBase
    {
        [ForeignKey(nameof(CategoriaServicio))]
        public Guid CategoriaServicioId { get; set; }
        public CategoriaServicio CategoriaServicio { get; set; } = null!;

        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; } // tiempo estimado

        public Guid PerfilBarberoId { get; set; }
        public PerfilBarbero PerfilBarbero { get; set; } = null!;
    }
}

