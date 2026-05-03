namespace API.Application.Dtos.Multibarbero.Afiliaciones
{
    public class GestionarAfiliacionInputDto
    {
        public Guid AfiliacionId { get; set; }
        public bool Activo { get; set; }
        public bool? EsPrincipal { get; set; }
        public string? MotivoFin { get; set; }
    }
}
