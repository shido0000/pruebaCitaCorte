namespace API.Application.Dtos.Multibarbero.Afiliaciones
{
    public class AfiliacionDto
    {
        public Guid Id { get; set; }
        public Guid BarberoId { get; set; }
        public string? BarberoNombre { get; set; }
        public string? BarberoFotoUrl { get; set; }
        public Guid BarberiaId { get; set; }
        public string? BarberiaNombre { get; set; }
        public DateTime FechaAfiliacion { get; set; }
        public bool Activo { get; set; }
        public bool EsPrincipal { get; set; }
        public DateTime? FechaFinAfiliacion { get; set; }
        public string? MotivoFin { get; set; }
    }
}
