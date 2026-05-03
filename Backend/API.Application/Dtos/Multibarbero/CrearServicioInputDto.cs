using System.ComponentModel.DataAnnotations;

namespace API.Application.Dtos.Multibarbero
{
    public class CrearServicioInputDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, 480, ErrorMessage = "La duración debe estar entre 1 y 480 minutos")]
        public int DuracionMinutos { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public Guid IdCategoriaServicio { get; set; }

        [Required(ErrorMessage = "La barbería es obligatoria")]
        public Guid IdBarberia { get; set; }
    }
}
