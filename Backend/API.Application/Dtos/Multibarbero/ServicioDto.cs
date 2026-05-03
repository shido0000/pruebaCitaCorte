namespace API.Application.Dtos.Multibarbero
{
    public class ServicioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }
        public Guid IdCategoriaServicio { get; set; }
        public string? CategoriaServicioNombre { get; set; }
        public Guid IdBarberia { get; set; }
        public string? BarberiaNombre { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
