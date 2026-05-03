using API.Data.Entidades.Seguridad;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entidades.Multibarbero
{
    /// <summary>
    /// Perfil del cliente que extiende Usuario
    /// </summary>
    public class PerfilCliente : EntidadBase
    {
        [ForeignKey(nameof(Usuario))]
        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public DateTime? FechaNacimiento { get; set; }
        public string? Genero { get; set; }
        public int TotalReservasRealizadas { get; set; }
        public decimal CalificacionPromedioDada { get; set; }
    }
}
