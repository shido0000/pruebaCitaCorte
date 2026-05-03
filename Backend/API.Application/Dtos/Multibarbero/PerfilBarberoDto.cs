using API.Application.Dtos.Comunes;

namespace API.Application.Dtos.Multibarbero.PerfilBarbero
{
    public class PerfilBarberoDto : EntidadBaseDto
    {
        public required string Nombre { get; set; }
        public required string Apellidos { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? FotoUrl { get; set; }
        public string? Biografia { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
