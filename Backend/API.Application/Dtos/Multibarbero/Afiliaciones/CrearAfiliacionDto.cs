namespace API.Application.Dtos.Multibarbero.Afiliaciones
{
    public class CrearAfiliacionDto
    {
        public Guid BarberoId { get; set; }
        public Guid BarberiaId { get; set; }
        public bool EsPrincipal { get; set; }
    }
}
